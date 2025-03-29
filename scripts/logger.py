import logging
from logging.handlers import TimedRotatingFileHandler
import sys, os

# Set up logging with TimedRotatingFileHandler
log_directory = "logs/"
log_filename = f"{log_directory}logfile"  # Log files will be in this directory

if not os.path.exists(log_directory):
    os.makedirs(log_directory)
    print(f"Directory '{log_directory}' created.")
else:
    print(f"Directory '{log_directory}' already exists.")
    

handler = TimedRotatingFileHandler(
    log_filename + ".log",  # Log file pattern, the handler will append the date
    when="midnight",  # Rotate every midnight
    interval=1,  # Rotate every 1 day
    backupCount=7  # Keep the last 7 log files
)

# Set up logging configuration
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(levelname)s - %(message)s',
    handlers=[handler]
)

# Redirecting print to logging
class PrintLogger:
    def write(self, message):
        if message.strip():  # Avoid logging empty lines
            logging.info(message.strip())

    def flush(self):
        pass

# Replace sys.stdout with our custom PrintLogger
sys.stdout = PrintLogger()

# Capture uncaught exceptions globally
def custom_excepthook(exc_type, exc_value, exc_tb):
    logging.error("Uncaught exception", exc_info=(exc_type, exc_value, exc_tb))

sys.excepthook = custom_excepthook
