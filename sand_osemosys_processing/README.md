# Script to process OSeMOSYS data files from SAND

`sand_filter_v2.py` is a Python script that:
1. Removes lines of parameter data that only contain default values for that parameter
2. Reformats the file to be compatible with the RES generator on the OSeMOSYS Cloud platform

## Usage
`python sand_filter_v2.py [data_in.txt] [data_out.txt]`
