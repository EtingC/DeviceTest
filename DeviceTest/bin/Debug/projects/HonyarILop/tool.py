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


BASE_HTTP_GET_URL = u"https://smart.hongyancloud.com/link?"

def generateRandomValue():
    return "00"

def generateDateMark():
    year = (datetime.datetime.now().year - 2018)+1
    month = generateCode(datetime.datetime.now().month)
    day = generateCode(datetime.datetime.now().day)
    value = "%X%s%s" % (year,month,day)
    return value


def generateDeviceName(device):
    deviceName = "H"  #固定头
    deviceName += "00" #预留
    deviceName += generateRandomValue() #随机码 默认为00
    deviceName += generateDateMark()#  2018为基准年   2018-10-11  转换为 1AB
    deviceName += "00" #生产工厂编号
    deviceName += "0"  # 预留
    deviceName += "%05d"  % int(device.SN)
    return deviceName


def process():
    path = os.path.dirname(os.path.realpath(__file__))
    labelContent = LabelContent()
    try:
        try:
            argumentFile = codecs.open("%s\\argument.txt" % path)#,encoding="utf-8")
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

        pair = KeyValuePair("MAC",device.deviceId)
        labelContent.Items.append(pair)

        pair = KeyValuePair("PTYPE",device.name)
        labelContent.Items.append(pair)

        pair = KeyValuePair("MODELID", device.modelId)
        labelContent.Items.append(pair)

        pair = KeyValuePair("ZIGBEEVER", device.softwareVersion)
        labelContent.Items.append(pair)

        pair = KeyValuePair("MCUVER", device.mcuBinVersion)
        labelContent.Items.append(pair)

        pair = KeyValuePair("DATE", NowDateToStr())
        labelContent.Items.append(pair)

        pair = KeyValuePair("SN", device.SN)
        labelContent.Items.append(pair)

        pair = KeyValuePair("SERIALCODE", device.serialCode)
        labelContent.Items.append(pair)
        # url = https: // smart.hongyancloud.com / link?mcode = 物料代码 & sn = XXXXXXXXXXXX & info = 私有信息

        url = BASE_HTTP_GET_URL+"mcode="+device.serialCode
        url += "&sn=" + device.SN
        url += "&mac="+ device.deviceId

        pair = KeyValuePair("QRCODE", url)
        labelContent.Items.append(pair)

        labelContent.FileURL = device.labelFilePathName
        labelContent.Message= u'生成标签成功'
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


