import logger
from idle_listener import IdleListener
import time, infra
import pygetwindow as gw
import pyautogui
import threading
import pynput
from position import Position
from mouse_listener import MouseListener
from keyboard_listener import KeyboardListener
from pycaw.pycaw import AudioUtilities, IAudioEndpointVolume

pyautogui.FAILSAFE = False

class Main():

    def __init__(self):

        self.position_instance = Position()
        self.mouse_listener_instance = MouseListener()
        self.keyboard_listener_instance = KeyboardListener()

        self.idle_listener_instance = IdleListener(self.on_idle, self.after_idle)
        self.idle_listener_instance.start_listener()

    def move_mouse_per_second(self, position, duration):
        x, y = position
        pyautogui.moveTo(x, y, duration)

    # Function to check if the mouse position deviates significantly from expected position
    def is_manual_move(self, expected_position, actual_position, threshold=50):
        expected_x, expected_y = expected_position
        actual_x, actual_y = actual_position
        
        # Calculate the difference between the expected and actual positions
        diff_x = abs(expected_x - actual_x)
        diff_y = abs(expected_y - actual_y)
        
        # If the difference exceeds the threshold, consider it as a manual move
        if diff_x > threshold or diff_y > threshold:
            return True  # Manual movement detected
        return False  # No manual movement
    

    def get_all_positions(self, start_x, start_y, end_x, end_y, steps=100):
        positions = []
        # Calculate the difference in x and y
        delta_x = end_x - start_x
        delta_y = end_y - start_y
        
        for i in range(steps + 1):  # steps + 1 to include the end position
            # Calculate the intermediate positions
            intermediate_x = start_x + (delta_x * i / steps)
            intermediate_y = start_y + (delta_y * i / steps)
            positions.append((int(intermediate_x), int(intermediate_y)))
        
        return positions
    
    def move_and_get_positions(self, position, steps=10):

        
        start_x, start_y = pyautogui.position()
        end_x, end_y = position

        # Get all positions between start and end
        all_positions = self.get_all_positions(start_x, start_y, end_x, end_y, steps)

        for pos in all_positions:
            pyautogui.press('shift')
            pyautogui.moveTo(pos[0], pos[1])  # Move the mouse to the position
            actual_position = pyautogui.position()

            if self.is_manual_move(pos, actual_position):
                print(f"Manual move detected! Expected: {pos}, Actual: {actual_position}")
                return None, 1 , None
            elif self.mouse_listener_instance.mouse_clicked_detected :
                print(f"Mouse clicked detected!")
                return None, 2 , self.mouse_listener_instance
            elif self.keyboard_listener_instance.keyboard_clicked_detected :
                print(f"Keyboard clicked detected!")
                return None, 3, self.keyboard_listener_instance

            time.sleep(0.01)  # You can adjust the sleep for a smoother movement

        return all_positions, None, None
    
    def start_mouse_move(self):

        self.mouse_listener_instance.mouse_clicked_detected = False
        self.keyboard_listener_instance.keyboard_clicked_detected = False

        self.mouse_move_listener, self.mouse_move_thread = self.mouse_listener_instance.start()
        self.keyboard_move_listener, self.keyboard_move_thread = self.keyboard_listener_instance.start()

        while True:

            if self.is_audio_playing():
                print("Audio is playing")
                continue

            value, _type, state = self.move_and_get_positions(Position.center())

            if value is None:
                break

            value, _type, state = self.move_and_get_positions(self.position_instance.get_heading_towards_position())
            
            if value is None:
                break

            self.position_instance.position_index = self.position_instance.position_index + 1

        self.stop_mouse_move()
        self.idle_listener_instance.start_listener()

        
    def stop_mouse_move(self):
        self.mouse_move_listener.stop()
        self.keyboard_move_listener.stop()

        self.mouse_move_thread.join()
        self.keyboard_move_thread.join()


    def is_audio_playing(self):
        sessions = AudioUtilities.GetAllSessions()
        for session in sessions:
            if session.Process:
                # Check if the session is playing sound (if it's not muted)
                if session.State == 1:  # Audio playing (State = 1)
                    return True
        return False

    def on_idle(self, idle_listener_instance):
        print("on Idle")
        idle_listener_instance.stop_listener()
        self.start_mouse_move()


    def after_idle(self, idle_listener_instance):
        print("after Idle")



print("--- Started ---")
Main()
print("=== Stopped ===")