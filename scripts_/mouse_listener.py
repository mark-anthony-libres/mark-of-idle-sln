from pynput.mouse import Listener
import threading

class MouseListener:

    def __init__(self):
        self.mouse_clicked_detected = False

    def on_click(self, x, y, button, pressed):
        if pressed:
            print(f"Mouse clicked at ({x}, {y})")
            self.mouse_clicked_detected = True  # Set the flag to True when a click is detected

    def on_scroll(self, *args):
        self.mouse_clicked_detected = True

    def start_mouse_listener(self):
        return Listener(on_click=self.on_click, on_scroll=self.on_scroll)

    def start(self):

        listener = self.start_mouse_listener()
        thread = threading.Thread(target=listener.start)
        thread.daemon = True  # This makes sure the thread will exit when the main program exits
        thread.start()

        return listener, thread