import os, sys
import pandas as pd
from collections import defaultdict
import re

def main(datafile_in, datafile_out):

    lines = []
    param_line = []
    param_current = []
    parsing = False
    index_line = []
    year_line = []
    index_tag = []
    year_tag = []

    with open(datafile_in, 'r') as file_in:
        for line in file_in:
            line = line.strip('\t ').replace('\t', ' ')
            line = re.sub(' +', ' ', line)
            line = line.replace(' \n', '\n')

            if line.startswith('set YEAR'):
                start_year = line.split(':=')[1].split(' ')[1]
                # print(start_year)

            if line.startswith('param'):
                parsing = True

                if line.startswith('param REMinProductionTarget'):
                    line = line.replace(':', ':=')

            if parsing:
                line_values = []
                if line.startswith('param'):
                    param_current = line.split(' ')[1]
                    line_elements = list(line.split(' '))
                    line_elements = [i.strip("\n:=") for i in line_elements]
                    lines.append(line)

                    if 'default' in line_elements:
                        default_index = line_elements.index('default')  # Find position of 'default'
                        default_value = line_elements[default_index+1]  # Extract default value
                        # print(param_current, default_value)

                elif line.startswith('['):
                    index_line = line
                    param_reset = True
                    index_tag = True

                elif line.startswith(str(start_year)):
                    year_line = []
                    year_line = line
                    param_reset = True
                    year_tag = True

                else:
                    line_values = list(set(line.rstrip('\n').split(' ')[1:-1]))

                    if line_values != list(default_value):
                        if line_values != []:
                            param_line = line

                    if len(param_line) > 0:
                        if param_reset:
                            if index_tag:
                                if index_line != []:
                                    lines.append(index_line)
                            if year_tag:
                                if year_line != []:
                                    lines.append(year_line)
                        param_reset = False
                        if not param_reset:
                            lines.append(param_line)
                            param_line = []
            if line.startswith(';'):
                lines.append(line)
                # print(param_current, index_tag, year_tag)
                parsing = False
                index_tag = False
                year_tag = False
            elif not parsing:
                lines.append(line)

    with open(datafile_out, 'w') as file_out:
        file_out.writelines(lines)


if __name__ == '__main__':

    if len(sys.argv) != 3:
        msg = "Usage: python {} <infile> <outfile>"
        print(msg.format(sys.argv[0]))
        sys.exit(1)
    else:
        datafile_in = sys.argv[1]
        datafile_out = sys.argv[2]
        main(datafile_in, datafile_out)
