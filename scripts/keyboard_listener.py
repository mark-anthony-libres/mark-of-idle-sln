from pynput.keyboard import Listener, Key
import threading

class KeyboardListener:

    def __init__(self):
        self.keyboard_clicked_detected = False


    def on_key_press(self, key):
        if not (key == Key.shift or key == Key.shift_r or key == Key.shift_l):
            print("keboard pressed")
            self.keyboard_clicked_detected = True

    def start_keyboard_listener(self):
        return Listener(on_press=self.on_key_press) 

    def start(self):

        listener = self.start_keyboard_listener()
        thread = threading.Thread(target=listener.start)
        thread.daemon = True  # This makes sure the thread will exit when the main program exits
        thread.start()

        return listener, thread