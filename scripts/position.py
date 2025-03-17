import pyautogui

class Position:

    def __init__(self):

        self.position_list = (
            Position.top_left,
            Position.top_right,
            Position.bottom_left,
            Position.bottom_right
        )

        self.position_index = 0

    def get_heading_towards_position(self):
        if self.position_index > (len(self.position_list) - 1):
            self.position_index = 0

        current = self.position_list[self.position_index]()
        return current

    @staticmethod
    def center():
        print("center")
        screen_width, screen_height = pyautogui.size()  # Get screen size
        return  screen_width // 2 , screen_height // 2

    @staticmethod
    def top_left():
        print("top left")
        return 1, 1
    
    @staticmethod
    def top_right():
        print("top right")
        screen_width, screen_height = pyautogui.size()
        return screen_width - 1, 2
    
    @staticmethod
    def bottom_left():
        print("bottom left")
        screen_width, screen_height = pyautogui.size()
        return 1, screen_height - 2
    
    @staticmethod
    def bottom_right():
        print("bottom right")

        screen_width, screen_height = pyautogui.size()
        return screen_width - 2, screen_height - 2
    