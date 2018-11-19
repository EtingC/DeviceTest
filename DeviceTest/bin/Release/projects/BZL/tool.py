# encoding=utf8
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


def process(argv):
    path = os.path.dirname(os.path.realpath(__file__))
    result = {}
    try:
        try:
            devicesFile = codecs.open("%s/devices.txt" % path)
            deviceStr = devicesFile.read()
            devicesFile.close()
        except:
            result["MESSAGE"] = u"Load error  [devices.txt]"
            result["ERRORCODE"] =ERROR_CODE_LOAD_FILE_FAIL
            return PythonDirToJson(result)

        try:
            configFile = codecs.open("%s/config.txt" % path)
            ConfigStr = configFile.read()
            configFile.close()
        except:
            result["MESSAGE"] = u"Load error [config.txt]"
            result["ERRORCODE"] =ERROR_CODE_LOAD_FILE_FAIL
            return PythonDirToJson(result)

        isBreak = False

        try:
            argumentFile = codecs.open("%s/argument.txt" % path)
            argument = argumentFile.read()

            argumentFile.close()
        except:
            result["MESSAGE"] = u"Load error [argument.txt]"
            result["ERRORCODE"] =ERROR_CODE_LOAD_FILE_FAIL
            return PythonDirToJson(result)

        try:
            device = ArgumentDevice()
            device.assign(argument)
        except Exception as er:
            result["MESSAGE"] = u"Invalid device arguments"
            result["ERRORCODE"] =ERROR_CODE_FAIL
            return PythonDirToJson(result)

        devShortModel = device.modelId
        devColor =  "%d" % device.deviceColor
        Sn = device.deviceId

        devices = JsonToPythonDir(deviceStr)
        qrcode = None
        Config = JsonToPythonDir(ConfigStr)
        for dev in devices['devices']:
            if not isBreak:
                qrcode = DirToObj(dev)
                shortmodels = qrcode.shortmodel.split(',')
                for shortmodel in shortmodels:
                    if shortmodel.find(devShortModel) >=  0:
                        qrcode.shortmodel = devShortModel
                        isBreak = True
                        break
                if not isBreak:
                    qrcode = None
            else:
                break

        if (not isBreak) or (qrcode == None):
            result["MESSAGE"] =  u'Unsupported modelId ['+devShortModel+']'
            result["ERRORCODE"] =ERROR_CODE_FAIL
            return PythonDirToJson(result)

        isBreak = False

        key = qrcode.deviceMode
        for colors in Config['ColorConfig']:
            if not isBreak:
                if key in colors:
                    if devColor in colors[key]:
                        qrcode.deviceStyle = colors[key][devColor]
                        isBreak = True
            else:
                break
        if not isBreak:
            result["MESSAGE"] = u'Unsupported device style'
            result["ERRORCODE"] = ERROR_CODE_FAIL
            return PythonDirToJson(result)

        qrcode.Sn = Sn
        qrcodeJson = ObjToDir(qrcode)
        del qrcodeJson['shortmodel']

        result['MAC'] = Sn
        result['PTYPE'] = Config['PTYPEConfig'][qrcode.deviceStyle]
        result.setdefault('QRCODE', qrcodeJson)

        result["MESSAGE"] = u'OK'
        result["ERRORCODE"] = ERROR_CODE_OK
        return PythonDirToJson(result)

    except Exception as err:
        result["MESSAGE"] = u'Unknown error'
        result["ERRORCODE"] = ERROR_CODE_FAIL
        return PythonDirToJson(result)


class QRCode(ItemObject):
    def __init__(self):
        self.deviceMode = 0
        self.deviceSupplier = "1"
        self.deviceType  =""
        self.Sn = ""
        self.deviceStyle = ""



def processNew():
    path = os.path.dirname(os.path.realpath(__file__))
    result = {}
    labelContent = LabelContent()
    try:
        try:
            devicesFile = codecs.open("%s/devices.txt" % path)
            deviceStr = devicesFile.read()
            devicesFile.close()
        except:
            labelContent.Message = u"载入文件 [devices.txt]错误"
            labelContent.ErrorCode = ERROR_CODE_LOAD_FILE_FAIL
            return labelContent.dumpToJSON()

        try:
            configFile = codecs.open("%s/config.txt" % path)
            ConfigStr = configFile.read()
            configFile.close()
        except:
            labelContent.Message = u"载入文件 [config.txt]错误"
            labelContent.ErrorCode = ERROR_CODE_LOAD_FILE_FAIL
            return labelContent.dumpToJSON()

        isBreak = False

        try:
            argumentFile = codecs.open("%s/argument.txt" % path)
            argument = argumentFile.read()
            argumentFile.close()
        except:
            labelContent.Message = u"载入文件 [argument.txt]错误"
            labelContent.ErrorCode = ERROR_CODE_LOAD_FILE_FAIL
            return labelContent.dumpToJSON()

        try:
            device = ArgumentDevice()
            device.assign(argument)
        except Exception as er:

            labelContent.Message = u"无效的设备参数"
            labelContent.ErrorCode = ERROR_CODE_FAIL
            return labelContent.dumpToJSON()

        devShortModel = device.modelId
        devColor =  "%d" % device.deviceColor
        Sn = device.deviceId

        devices = JsonToPythonDir(deviceStr)
        qrcode = None
        Config = JsonToPythonDir(ConfigStr)
        for dev in devices['devices']:
            if not isBreak:
                qrcode = DirToObj(dev)
                shortmodels = qrcode.shortmodel.split(',')
                for shortmodel in shortmodels:
                    if shortmodel.find(devShortModel) >=  0:
                        qrcode.shortmodel = devShortModel
                        isBreak = True
                        break
                if not isBreak:
                    qrcode = None
            else:
                break

        if (not isBreak) or (qrcode == None):
            labelContent.Message = u'不支持的ModelId ['+devShortModel+u']'
            labelContent.ErrorCode = ERROR_CODE_FAIL
            return labelContent.dumpToJSON()

        isBreak = False

        key = qrcode.deviceMode
        for colors in Config['ColorConfig']:
            if not isBreak:
                if key in colors:
                    if devColor in colors[key]:
                        qrcode.deviceStyle = colors[key][devColor]
                        isBreak = True
            else:
                break
        if not isBreak:
            labelContent.Message = u'不支持的设备Style'
            labelContent.ErrorCode = ERROR_CODE_FAIL
            return labelContent.dumpToJSON()

        qrcode.Sn = Sn

        NewQRCode = QRCode()
        NewQRCode.Sn = Sn
        NewQRCode.deviceStyle = qrcode.deviceStyle
        NewQRCode.deviceMode = qrcode.deviceMode
        NewQRCode.deviceSupplier = qrcode.deviceSupplier
        NewQRCode.deviceType = qrcode.deviceType

        qrcodeJson = ObjToDir(qrcode)
        del qrcodeJson['shortmodel']

        pair = KeyValuePair("MAC",Sn)
        labelContent.Items.append(pair)

        pair = KeyValuePair("PTYPE",Config['PTYPEConfig'][qrcode.deviceStyle])
        labelContent.Items.append(pair)

        pair = KeyValuePair("QRCODE",NewQRCode.dumpToJSON())
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
    result = processNew()
    sys.stdout.write(result.encode(encoding='gbk'))
    sys.exit()
