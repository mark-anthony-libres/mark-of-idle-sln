import time, infra
import threading
from pynput import mouse, keyboard

class IdleListener:


    def __init__(self, on_idle, after_idle):

        self.IDLE_THRESHOLD = infra.get('threshold')
        self.last_activity_time = time.time()
        self.is_idle = False
        self.on_idle = on_idle
        self.after_idle = after_idle

    def on_move(self, x, y):
        self.last_activity_time = time.time()  

    def on_click(self, x, y, button, pressed):
        if pressed:
            self.last_activity_time = time.time() 

        
    def on_scroll(self, x, y, dx, dy):
        self.last_activity_time = time.time()
    
    def on_press(self, key):
        self.last_activity_time = time.time()


    # Function to check if the system is idle
    def check_idle(self):

        while infra.get('is_active'):
            # Calculate idle time
            idle_time = time.time() - self.last_activity_time

            print(idle_time)
            
            if idle_time > self.IDLE_THRESHOLD:
                if not self.is_idle:
                    print("---->>> System is now idle!")
                    self.is_idle = True  # Mark the system as idle
                    self.on_idle(self)
            else:
                if self.is_idle:
                    print("---->>>  System is no longer idle!")
                    self.is_idle = False  # Mark the system as active again
                    self.after_idle(self)
            
            # Check every second
            time.sleep(1)


    def start_mouse_listener(self):
        return mouse.Listener(on_move=self.on_move, on_click=self.on_click, on_scroll=self.on_scroll)

    def start_keyboard_listener(self):
        return keyboard.Listener(on_press=self.on_press)
    
    def start_listener(self):

        self.is_idle = False
        self.active_threads = []
        self.last_activity_time = time.time() 

        _listeners = [
            self.start_mouse_listener(),
            self.start_keyboard_listener()
        ]

        for per in _listeners:
            listener_thread = threading.Thread(target=per.start)  # Start listener in a separate thread
            listener_thread.daemon = True  # Ensures that the thread exits when the main program exits
            listener_thread.start()

            self.active_threads.append({
                "listener" : per,
                "thread" : listener_thread
            })

        self.check_idle()

        return self.active_threads

    def stop_listener(self):

        for per in self.active_threads:
            per['listener'].stop()
            per['thread'].join()

        self.active_threads = []


    