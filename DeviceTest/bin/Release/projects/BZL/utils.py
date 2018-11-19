# encoding=utf8


import json
import datetime
import os
import codecs

ERROR_CODE_OK = 0
ERROR_CODE_FAIL = 1
ERROR_CODE_LOAD_FILE_FAIL =2



def convertToJSON(obj):
    d = {}
    d.update(obj.__dict__)
    return d


def dumpToJSON(item):
    return json.dumps(item, default=convertToJSON, ensure_ascii=False)




class ItemObject(object):
    def dumpToJSON(self):
        return dumpToJSON(self)


class ResultObject(ItemObject):
    def __init__(self):
        ItemObject.__init__(self)
        self.errorCode = ERROR_CODE_FAIL
        self.lastMessage = ""


class ObjectController(ItemObject):
    def assignFromDict(self,args):
        return True
    def assignFromList(self,args):
        return True
    def assign(self,response):
        if isinstance(response,str) :#or isinstance(response,unicode):
            datas = json.loads(response,encoding="utf-8")
            if isinstance(datas,dict):
                args = dict((key, value) for key, value in datas.items())
                return self.assignFromDict(args)
            elif isinstance(datas,list):
               return self.assignFromList(datas)
        elif isinstance(response,dict):
           return self.assignFromDict(response)


class ArgumentDevice(ObjectController):
    def __init__(self):
        self.name = ""
        self.model = ""
        self.deviceId = ""
        self.modelId = ""
        self.secret = ""
        self.subDeviceType = 0
        self.hardwareVersion = ""
        self.softwareVersion = ""
        self.mcuBinVersion = ""
        self.deviceColor = 1
        self.SN = ""
        self.labelFilePathName =""
    def assignFromDict(self,args):
        self.name = args.get("name","")
        self.model = args.get("model","")
        self.deviceId = args.get("deviceId","")
        self.modelId = args.get("modelId","")
        self.secret = args.get("secret","")
        self.subDeviceType = args.get("subDeviceType",0)
        self.hardwareVersion = args.get("hardwareVersion","")
        self.softwareVersion = args.get("softwareVersion","")
        self.mcuBinVersion = args.get("mcuBinVersion","")
        self.deviceColor = args.get("deviceColor",1)
        self.SN = args.get("SN","")
        self.labelFilePathName = args.get("labelFilePathName","")


class ResultItem(ResultObject):
    def __init__(self):
        ResultObject.__init__(self)
        self.Context = ""

def DatetimeToStr(datetimeInfo):
    return datetimeInfo.strftime('%Y-%m-%d %H:%M:%S')


def NowDateToStr():
    return datetime.datetime.now().strftime('%Y-%m-%d')


# Python Dic To JSON
def PythonDirToJson(data):
    json_str = json.dumps(data)
    return json_str


#JSON To Python Dic
def JsonToPythonDir(json_str):
    dataDir = json.loads(json_str)
    return dataDir


# class To dict
def ObjToDir(obj):
    pr = {}
    for name in dir(obj):
        value = getattr(obj, name)
        if not name.startswith('__') and not callable(value):
            pr[name] = value
    return pr


# dict To object
def DirToObj(Dir):
    top = type('new', (object,), Dir)
    seqs = tuple, list, set, frozenset
    for i, j in Dir.items():
        if isinstance(j, dict):
            setattr(top, i, DirToObj(j))
        elif isinstance(j, seqs):
            setattr(top, i,
                    type(j)(DirToObj(sj) if isinstance(sj, dict) else sj for sj in j))
        else:
            setattr(top, i, j)
    return top


class KeyValuePair(object):
    def __init__(self,key,value):
        self.Key = key
        self.Value = value

class LabelContent(ItemObject):
    def __init__(self):
        ItemObject.__init__(self)
        self.Message = ""
        self.ErrorCode = 0
        self.FileURL = ""
        self.Items  =[]


