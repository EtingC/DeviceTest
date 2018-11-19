#encoding=utf8
# ***********************************************
#
#      Filename: tool.py
#
#        Author: Benson - zjxucb@gmail.com
#   Description: ---
#        Create: 2018-04-11 09:39:36
# Last Modified: 2018-04-11 09:39:39
# ***********************************************

import codecs
import sys
import os

from utils import *




class DeviceItem(ItemObject):
    def __init__(self,modelId,name):
        ItemObject.__init__(self)
        self.modelId = modelId
        self.name = name

class DeviceItems(ItemObject):
    def __init__(self):
        ItemObject.__init__(self)
        self.items= []
    def LoadFromFile(self,filename):
        # try:
            file = codecs.open(filename)
            lines = file.readlines()
            file.close()
            for line in lines:
                line = line.strip()
                list = line.split(" ")
                if len(list)>=2:
                    deviceItem= DeviceItem(list[0],list[1])
                    self.items.append(deviceItem)
            return True
        # except:
        #     return False

    def GetName(self,modelId):
        for item in self.items:
            itemModelId = item.modelId
            modelId = modelId.encode(encoding='utf-8')
            if  modelId in itemModelId :
                return item.name
        return ""


def process():
    path = os.path.dirname(os.path.realpath(__file__))
    labelContent = LabelContent()
    try:
        try:
            argumentFile = codecs.open("%s\\argument.txt" % path)
            argument = argumentFile.read()
            argumentFile.close()
        except Exception as er:
            labelContent.Message= u"载入文件 [argument.txt]错误"
            labelContent.ErrorCode =ERROR_CODE_LOAD_FILE_FAIL
            return labelContent.dumpToJSON()

        try:
            device = ArgumentDevice()
            device.assign(argument)
        except Exception as er:
            labelContent.Message = u"无效的设备参数文件"
            labelContent.ErrorCode = ERROR_CODE_FAIL
            return labelContent.dumpToJSON()

        deviceItems = DeviceItems()
        result =deviceItems.LoadFromFile("%s\\device.txt" % path)
        if not result:
            labelContent.Message = u"载入文件[device.txt]错误"
            labelContent.ErrorCode = ERROR_CODE_LOAD_FILE_FAIL
            return labelContent.dumpToJSON()

        newType = deviceItems.GetName(device.modelId)
        if newType == "":
            labelContent.Message = u'不支持的ModelId ['+device.modelId+u']'
            labelContent.ErrorCode = ERROR_CODE_FAIL
            return labelContent.dumpToJSON()

        pair = KeyValuePair("PTYPE", newType)
        labelContent.Items.append(pair)

        mac = JoinChar(device.deviceId,':',2)
        pair = KeyValuePair("MAC",mac)
        labelContent.Items.append(pair)
        pair = KeyValuePair("SN",device.SN)
        labelContent.Items.append(pair)



        labelContent.FileURL = device.labelFilePathName
        labelContent.Message = u'生成标签成功'
        labelContent.ErrorCode = ERROR_CODE_OK
        return labelContent.dumpToJSON()

    except Exception as err:
        labelContent.Message = u'未知的错误'
        labelContent.ErrorCode = ERROR_CODE_FAIL
        return labelContent.dumpToJSON()

if __name__ == '__main__':
    result = process()
    sys.stdout.write(result.encode(encoding='gbk'))
    sys.exit()

