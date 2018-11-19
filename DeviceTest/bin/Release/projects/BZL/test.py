#-*- coding: utf-8 -*-
#***********************************************
#
#      Filename: test.py
#
#        Author: Benson - zjxucb@gmail.com
#   Description: ---
#        Create: 2018-04-11 09:39:36
# Last Modified: 2018-04-11 09:39:39
#***********************************************

import unittest
import xlrd
import json

from  tool import main

# 将 JSON To Python Dic
def JsonToPythonDir(json_str):
    dataDir = json.loads(json_str)
    # print("dataDir ==> ",dataDir)
    return dataDir

def open_excel(file= 'file.xls'):
    try:
        data = xlrd.open_workbook(file)
        return data
    except Exception as e:
        print(str(e))

def excel_table_byname(file= u'鸿雁设备DeviceID对照表-20180315.xlsx',colnameindex=1,by_name=u'Device ID'):
    data = open_excel(file)
    table = data.sheet_by_name(by_name) 
    nrows = table.nrows
    colnames = table.row_values(colnameindex)
    list = []
    for rownum in range(2, nrows):
        row = table.row_values(rownum)
        if row:
            app = {}
            for i in range(len(colnames)):
                app[colnames[i]] = row[i]
            list.append(app)
    return list

class TestCase(unittest.TestCase):
    def test_10(self):
        result = main(["",'00500c32','1','1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_11(self):
        result = main(["",'00500c32','1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_12(self): 
        result = main(["", 'HY0030', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_13(self): 
        result = main(["", '005f0cf1', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_14(self): 
        result = main(["", 'HY0036', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_15(self): 
        result = main(["", '', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_16(self): 
        result = main(["", '005f0cf2', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_17(self): 
        result = main(["", '005f0cf3', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_18(self): 
        result = main(["", 'HY0037', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_19(self): 
        result = main(["", '00500c35', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_20(self): 
        result = main(["", 'HY0032', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_21(self): 
        result = main(["", '005f0cf2', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_22(self): 
        result = main(["", 'HY0038', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_23(self): 
        result = main(["", '000a0c3c', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_24(self): 
        result = main(["", 'RH3002', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        print(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']
                break 
        self.assertEqual(result, LPapp)
    def test_25(self): 
        result = main(["", '000a0c55', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_26(self): 
        result = main(["", 'HY0043', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_27(self): 
        result = main(["", '005f0c3b', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        # print(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):
                # print(app)
                LPapp =  app['输出']
        self.assertEqual(result, LPapp)
    def test_28(self): 
        result = main(["", 'HY0041', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_29(self): 
        result = main(["", '000212c3', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        # print(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_30(self): 
        result = main(["",  'HY0080',  '2',  '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_31(self): 
        result = main(["",  '005e0edb',  '1',  '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        # print(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_32(self): 
        result = main(["",  'HY0081',  '2',  '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_33(self): 
        result = main(["", '00280eda', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        # print(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_34(self): 
        result = main(["", 'HY0082', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_35(self): 
        result = main(["", '00010d0d', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_36_1(self): 
        result = main(["", '00330dd8', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']
                break   
        self.assertEqual(result, LPapp)
    def test_36_2(self): 
        result = main(["", 'RH3002', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']
                break   
        self.assertEqual(result, LPapp)
    def test_37_1(self): 
        result = main(["", '001a0e0e', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_37_2(self): 
        result = main(["", 'RH3041', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_38(self): 
        result = main(["",'HY0090','1','1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_39(self): 
        result = main(["", '00040c3a', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_40(self): 
        result = main(["", 'HY0039', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_41(self): 
        result = main(["", '005f0d13', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_42(self): 
        result = main(["", 'HY0040', '2', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_43_1(self): 
        result = main(["", '0001112b', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)
    def test_43_2(self): 
        result = main(["", 'RH8080', '1', '1234567890ABCDEF'])
        json = JsonToPythonDir(result)
        PTYPE = json['PTYPE']
        resultexlsx  =  excel_table_byname()
        for app in resultexlsx:
            if(app['PTYPE'] == PTYPE):  
                LPapp =  app['输出']   
        self.assertEqual(result, LPapp)

if __name__ == '__main__':
    unittest.main()

