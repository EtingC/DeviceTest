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


class QRCode(ItemObject):
    def __init__(self,device):
        ItemObject.__init__(self)
        self.MAC = device.deviceId
        self.PTYPE = device.name
        self.MODELID = device.modelId
        self.ZIGBEEVER = device.softwareVersion
        self.MCUVER =  device.mcuBinVersion
        self.DATE = NowDateToStr()
        self.SN = device.SN


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

        QRCodeItem =QRCode(device)

        pair = KeyValuePair("QRCODE", QRCodeItem.dumpToJSON())
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


