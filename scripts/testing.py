import argparse

# Function to handle the logic of your program
def main(args):
    if args.verbose:
        print("Verbose mode enabled")

    # Your program logic goes here
    if args.key:
        print(f"Key provided: {args.key}")
    
    if args.value:
        print(f"Value provided: {args.value}")

# Setting up argument parsing
def setup_argument_parser():
    parser = argparse.ArgumentParser(description="This is a sample program that accepts arguments.")
    
    # Define arguments
    parser.add_argument('--key', type=str, help="The key to be processed")
    parser.add_argument('--value', type=str, help="The value associated with the key")
    parser.add_argument('--verbose', action='store_true', help="Enable verbose output")

    return parser

if __name__ == "__main__":
    parser = setup_argument_parser()
    args = parser.parse_args()
    
    main(args)
