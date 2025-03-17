import json



file_path = 'data.json'

DEFAULT_THRESHOLD = 100
DEFAULT_IS_ACTIVE = False

def read_json():
    with open(file_path, 'r') as f:
        return json.load(f)
    

def set(key, new_value):
    data = read_json()  # Read current data from file
    data[key] = new_value

    with open(file_path, 'w') as f:
        json.dump(data, f, indent=4) 


def get(key):

    data = read_json()

    if key in data:
        return data[key]
    
    # If the key does not exist, try to get the predefined constant value dynamically
    default_key = f"DEFAULT_{key.upper()}"
    
    if default_key in globals():
        return globals()[default_key]
    
    raise KeyError(f"Key '{key}' not found in {file_path} and no default value found.")