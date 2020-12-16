import pandas as pd
pd.set_option('mode.chained_assignment', None)

import os, sys
import subprocess
from tkinter import filedialog
from tkinter import *
from collections import defaultdict

import zipfile

import itertools
import plotly.express as px
import plotly.graph_objects as go
import plotly.io as pio

import re


## Select input folder which contains data and results 

def main(year):
    root = Tk()
    root.folder =  filedialog.askdirectory()
    root.destroy()
    
    data_file = os.path.join(root.folder, 'data.txt')
    results_file = os.path.join(root.folder, "cbcoutput.txt")

    
    ## Replace TABS with single whitespace. Remove trailing whitespaces.
    lines = []
    with open(data_file, 'r') as file_in:
        for each_line in file_in:
            new_line = re.sub('\t', ' ', each_line)
            new_line = new_line.lstrip(' ')
            new_line = re.sub(' +', ' ', new_line)
            lines.append(new_line)
            
    with open(data_file, 'w') as file_out:
        file_out.writelines(lines)


    # Create \res\csv folder in current working directory if it doesn't already exist
    try:
        os.makedirs(os.path.join(root.folder, 'res\csv'))
    except FileExistsError:
        pass    

    lines = []

    with open(data_file, 'r') as f1:
        for line in f1:
            if not line.startswith(('set MODEper','set MODEx', 'end;')):
                lines.append(line)

    parsing = False
    parsing_year = False
    parsing_tech = False
    parsing_fuel = False
    parsing_mode = False
    parsing_storage = False
    parsing_emission = False

    year_list = []
    fuel_list = []
    tech_list = []
    storage_list = []
    mode_list = []
    emission_list = []

    data_all = []
    data_out = []
    data_inp = []
    output_table = []
    storage_to = []
    storage_from = []
    emission_table = []
    data_format = 'momani'

    param_check = ['OutputActivityRatio', 'InputActivityRatio', 'TechnologyToStorage', 'TechnologyFromStorage', 'EmissionActivityRatio']

    with open(data_file, 'r') as f:
        for line in f:
            if parsing_year:
                year_list += [line.strip()] if line.strip() not in ['', ';'] else []
            if parsing_fuel:
                fuel_list += [line.strip()] if line.strip() not in ['', ';'] else [] 
            if parsing_tech:
                tech_list += [line.strip()] if line.strip() not in ['', ';'] else [] 
            if parsing_storage:
                storage_list += [line.strip()] if line.strip() not in ['', ';'] else [] 
            if parsing_mode:
                mode_list += [line.strip()] if line.strip() not in ['', ';'] else [] 
            if parsing_emission:
                emission_list += [line.strip()] if line.strip() not in ['', ';'] else []

            if line.startswith('set YEAR'):
                if len(line.split('=')[1]) > 1:
                    year_list = line.split(' ')[3:-1]
                else:
                    parsing_year = True
            if line.startswith('set COMMODITY'):  # Extracts list of COMMODITIES from data file. Some models use FUEL instead. 
                if len(line.split('=')[1]) > 1:
                    fuel_list = line.split(' ')[3:-1]
                else:
                    parsing_fuel = True
            if line.startswith('set FUEL'):  # Extracts list of FUELS from data file. Some models use COMMODITIES instead. 
                if len(line.split('=')[1]) > 1:
                    fuel_list = line.split(' ')[3:-1]
                else:
                    parsing_fuel = True
            if line.startswith('set TECHNOLOGY'):
                if len(line.split('=')[1]) > 1:
                    tech_list = line.split(' ')[3:-1]
                else:
                    parsing_tech = True
            if line.startswith('set STORAGE'):
                if len(line.split('=')[1]) > 1:
                    storage_list = line.split(' ')[3:-1]
                else:
                    parsing_storage = True
            if line.startswith('set MODE_OF_OPERATION'):
                if len(line.split('=')[1]) > 1:
                    mode_list = line.split(' ')[3:-1]
                else:
                    parsing_mode = True
            if line.startswith('set EMISSION'):
                if len(line.split('=')[1]) > 1:
                    emission_list = line.split(' ')[3:-1]
                else:
                    parsing_emission = True

            if line.startswith(";"):
                parsing_year = False
                parsing_tech = False
                parsing_fuel = False
                parsing_mode = False
                parsing_storage = False
                parsing_emission = False

    start_year = year_list[0]


    if data_format == 'momani':
        with open(data_file, 'r') as f:
            for line in f:
                if line.startswith(";"):
                    parsing = False
                if parsing:
                    if line.startswith('['):
                        fuel = line.split(',')[2]
                        tech = line.split(',')[1]
                        emission = line.split(',')[2]
                    elif line.startswith(start_year):
                        years = line.rstrip(':= ;\n').split(' ')[0:]
                        years = [i.strip(':=') for i in years]
                    else:
                        values = line.rstrip().split(' ')[1:]
                        mode = line.split(' ')[0]

                        if param_current == 'OutputActivityRatio':    
                            data_out.append(tuple([fuel, tech, mode]))
                            for i in range(0, len(years)):
                                output_table.append(tuple([tech, fuel, mode, years[i], values[i]]))

                        if param_current == 'InputActivityRatio':
                            data_inp.append(tuple([fuel, tech, mode]))   

                        data_all.append(tuple([tech, mode]))

                        if param_current == 'TechnologyToStorage':
                            if not line.startswith(mode_list[0]):
                                storage = line.split(' ')[0]
                                values = line.rstrip().split(' ')[1:]
                                for i in range(0, len(mode_list)):
                                    if values[i] != '0':
                                        storage_to.append(tuple([storage, tech, mode_list[i]]))

                        if param_current == 'TechnologyFromStorage':
                            if not line.startswith(mode_list[0]):
                                storage = line.split(' ')[0]
                                values = line.rstrip().split(' ')[1:]
                                for i in range(0, len(mode_list)):
                                    if values[i] != '0':
                                        storage_from.append(tuple([storage, tech, mode_list[i]]))

                        if param_current == 'EmissionActivityRatio':
                            emission_table.append(tuple([emission, tech, mode]))

                if line.startswith(('param OutputActivityRatio', 'param InputActivityRatio', 'param TechnologyToStorage', 'param TechnologyFromStorage', 'param EmissionActivityRatio')):
                    param_current = line.split(' ')[1]
                    parsing = True


    #Read CBC output file
    df = pd.read_csv(results_file, sep='\t')
    #if str(df.iloc[0]).split(' ')[0] == "Infeasible":
    #    print("INFEASIBLE RESULT!  CHECK YOUR PARAMETERS!")
    #    exit(0) # Kill the kernel so we don't continue to run...

    if str(df.iloc[0]).split(' ')[0] == "Optimal":
        print("Optimal Solution Found.")
    df.columns = ['temp']
    df['temp'] = df['temp'].str.lstrip(' *\n\t')
    df[['temp','value']] = df['temp'].str.split(')', expand=True)
    df = df.applymap(lambda x: x.strip() if isinstance(x,str) else x)
    df['value'] = df['value'].str.split(' ', expand=True)
    df[['parameter','id']] = df['temp'].str.split('(', expand=True)
    df['parameter'] = df['parameter'].str.split(' ', expand=True)[1]
    df = df.drop('temp', axis=1)
    df['value'] = df['value'].astype(float).round(4)

    params = df.parameter.unique()
    all_params = {}
    cols = {'NewCapacity':['r','t','y'],
                'AccumulatedNewCapacity':['r','t','y'], 
                'TotalCapacityAnnual':['r','t','y'],
                'CapitalInvestment':['r','t','y'],
                'AnnualVariableOperatingCost':['r','t','y'],
                'AnnualFixedOperatingCost':['r','t','y'],
                'SalvageValue':['r','t','y'],
                'DiscountedSalvageValue':['r','t','y'],
                'TotalTechnologyAnnualActivity':['r','t','y'],
                'RateOfActivity':['r','l','t','m','y'],
                'RateOfTotalActivity':['r','t','l','y'],
                'Demand':['r','l','f','y'],
                'TotalAnnualTechnologyActivityByMode':['r','t','m','y'],
                'TotalTechnologyModelPeriodActivity':['r','t'],
                'ProductionByTechnologyAnnual':['r','t','f','y'],
                'ProductionByTechnology':['r','l','t','f','y'],
                'AnnualTechnologyEmissionByMode':['r','t','e','m','y'],
                'AnnualTechnologyEmission':['r','t','e','y'],
                'AnnualEmissions':['r','e','y'],
                'TotalInputToNewCapacity':['r','f','y'],
                'TotalInputToTotalCapacity':['r','f','y'],
                'TechnologyActivityChangeByModeCostTotal':['r','t','y']}

    for each in params:
        df_p = df[df.parameter == each]
        df_p[cols[each]] = df_p['id'].str.split(',',expand=True)
        cols[each].append('value')
        df_p = df_p[cols[each]] # Reorder dataframe to include 'value' as last column
        all_params[each] = pd.DataFrame(df_p) # Create a dataframe for each parameter
        df_p = df_p.rename(columns={'value':each})
        df_p.to_csv(os.path.join(root.folder, 'res\csv', str(each) + '.csv'), index=None) # Print data for each paramter to a CSV file

    year_split = []
    parsing = False

    with open(data_file, 'r') as f:
        for line in f:
            if line.startswith(";"):
                parsing = False   
            if parsing:
                if line.startswith(start_year):
                    years = line.rstrip().split(' ')[0:]
                    years = [i.strip(':=') for i in years]
                elif not line.startswith(start_year):
                    time_slice = line.rstrip().split(' ')[0]
                    values = line.rstrip().split(' ')[1:]
                    for i in range(0,len(years)):
                        year_split.append(tuple([time_slice,years[i],values[i]]))
            if line.startswith('param YearSplit'):
                parsing = True

    df_output = pd.DataFrame(output_table, columns=['t','f','m','y','OutputActivityRatio'])
    df_yearsplit = pd.DataFrame(year_split, columns=['l','y','YearSplit'])
    df_activity = all_params['RateOfActivity'].rename(columns={'value':'RateOfActivity'})

    df_out_ys = pd.merge(df_output, df_yearsplit, on='y')

    df_out_ys['OutputActivityRatio'] = df_out_ys['OutputActivityRatio'].astype(float)
    df_out_ys['YearSplit'] = df_out_ys['YearSplit'].astype(float)

    df_prod = pd.merge(df_out_ys, df_activity, on=['t','m','l','y'])

    df_prod['ProductionByTechnology'] = df_prod['OutputActivityRatio']*df_prod['YearSplit']*df_prod['RateOfActivity']
    df_prod = df_prod.drop(['OutputActivityRatio','YearSplit','RateOfActivity'], axis=1)

    #df_prod = df_prod.groupby(['r','t','f','y'])['ProductionByTechnology'].sum().reset_index()
    df_prod['ProductionByTechnology'] = df_prod['ProductionByTechnology'].astype(float).round(4)

    df_prod.to_csv(os.path.join(root.folder, 'res\csv', 'ProductionByTechnology.csv'), index=None)
    all_params['ProductionByTechnology'] = df_prod.rename(columns={'ProductionByTechnology':'value'})

    
    ## Read ProductionByTechnology
    df_hourly = pd.read_csv(os.path.join(root.folder,
                                'res',
                                'csv',
                                'ProductionByTechnology.csv'))

    ## Read TS definition file
    seasons_months_days = pd.read_csv(os.path.join(root.folder,
                                                'seasons.csv'), 
                                    encoding='latin-1')

    seasons_dict = dict([(m,s) for m,s in zip(seasons_months_days.MONTH, seasons_months_days.SEASON)])
    days_dict = dict([(m,d) for m,d in zip(seasons_months_days.MONTH, seasons_months_days.DAYS)])
    months  = list(seasons_dict)
    hours = list(range(1,25))
    years =list(df_hourly.y.unique())



    def ts_template():
        generation = ['ELC001']
        df_ts_template = pd.DataFrame(list(itertools.product(generation,
                                                                months,
                                                                hours,
                                                                years)
                                            ),
                                    columns = ['f', 'MONTH', 'HOUR', 'y']
                                    )        
        df_ts_template = df_ts_template.sort_values(by=['f', 'y'])
        df_ts_template['DAYS'] = df_ts_template['MONTH'].map(days_dict)
        df_ts_template['SEASON'] = df_ts_template['MONTH'].map(seasons_dict)
        df_ts_template['y'] = df_ts_template['y'].astype(int)
        
        
        return df_ts_template

    def plot_hourly(df, plot_title, x_title):
        fig = px.bar(df,
                    x='HOUR',
                    y=[x for 
                        x in df.columns
                        if x not in ['MONTH','HOUR']
                        ],
                    title=plot_title,
                    facet_col='MONTH',
                    )
        
        #return fig.write_html(os.path.join(root.folder,"hourly_generation.html"))
        return df
    
    df = df_hourly.loc[df_hourly.t.str.startswith('PWR')]
    df = df.loc[~df.t.str.contains('TRN')]
    df.t = df.t.str[3:6]

    df.ProductionByTechnology = df.ProductionByTechnology.astype('float64')
    df['SEASON'] = df['l'].str[1:2].astype(int)
    df['HOUR'] = df['l'].str[2:].astype(int)
    df['y'] = df['y'].astype(int)


    df.drop(['r', 'l'], 
            axis=1,
            inplace=True)

    df = pd.merge(df, 
                ts_template(),
                how = 'left',
                on = ['f','SEASON', 'HOUR', 'y']).dropna()



    #df['FUEL'] = df['FUEL'].str[0:2]
    max_year = max(df['y'])
    df = df[df['y'] == int(year)]

    df['ProductionByTechnology'] = (df['ProductionByTechnology'].mul(1e6))/(df['DAYS'].mul(3600))
    df = df.groupby(['MONTH','HOUR','t'], 
                    as_index=False)['ProductionByTechnology'].sum()
    df['MONTH'] = pd.Categorical(df['MONTH'], categories=months, ordered=True)
    df = df.sort_values(by=['MONTH', 'HOUR'])
    #df = df.rename(columns = tech_names)    

    fig = px.area(df,
                x='HOUR',
                y='ProductionByTechnology',
                #title=plot_title,
                facet_col='MONTH',
                title='Hourly generation for ' + str(year), 
                color='t',
                labels={
                    "variable": ""
                       }
                 )

    fig.for_each_annotation(lambda a: a.update(text=a.text.split("=")[-1]))

    for axis in fig.layout:
            if type(fig.layout[axis]) == go.layout.XAxis:
                fig.layout[axis].title.text = ''
            
            fig['layout']['yaxis']['title']['text']='Gigawatts (GW)'
            fig['layout']['yaxis3']['title']['text']=''
            fig['layout']['xaxis']['title']['text']=''
            fig['layout']['xaxis6']['title']['text']='Hours'
        
    #fig.show()
    fig.write_html(os.path.join(root.folder,"hourly_generation.html"))

if __name__ == "__main__":
    if len(sys.argv) != 2:
        msg = "Usage: python {} YEAR"
        print(msg.format(sys.argv[0]))
        sys.exit(1)
    else:

        year = sys.argv[1]
        main(year)
