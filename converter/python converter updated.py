# Code is updated
# Date: 14th March 2023
import pandas as pd
import numpy as np
import sys
from pathlib import Path

if __name__ == "__main__":

    input = sys.argv[0] # changed the argv[1] to argv[0]
    output_dir = sys.argv[0] # changed the argv[1] to argv[0]

    output_filename = input.split("\\")[-1] + ".processed_results.csv"
    output_dir = r"{}".format(output_dir)
    input = r"{}".format(input)

    columns = [
        "index",
        "Variable",
        "Dim1",
        "Dim2",
        "Dim3",
        "Dim4",
        "Dim5",
        "Dim6",
        "Dim7",
        "Dim8",
        "Dim9",
        "Dim10",
        "ResultValue",
    ]

    osemosys_output = pd.read_csv(
        input, names=columns, sep="\(|,|\)|[ \t]{1,}", engine="python"
    )
    osemosys_output = osemosys_output[osemosys_output["index"] != "Optimal"]

    def check_for_all_zeros(df):
        df.loc[:, (df == 0).all(axis=0)] = np.nan
        return df

    osemosys_clean = osemosys_output.groupby("Variable").apply(
        lambda x: check_for_all_zeros(x)
    )

    osemosys_clean["ResultValue"] = osemosys_clean.ffill(axis=1).iloc[:, -1]

    def replace_result_value(df):
        df[df.loc[:, ~df.isnull().all()].iloc[:, -2].name] = np.nan
        return df

    osemosys_cleaned = osemosys_clean.groupby("Variable").apply(
        lambda x: replace_result_value(x)
    )

    osemosys_cleaned = osemosys_cleaned.drop("index", axis=1)

    output_directory = Path(output_dir) / Path(output_filename)

    # output_directory = '"{}"'.format(output_directory)
    osemosys_clean.to_csv(r'C:\Users\biswa\Downloads\clicSAND\converter\output_directory.csv'
        ,
        index=False,encoding='utf-8'
    )
