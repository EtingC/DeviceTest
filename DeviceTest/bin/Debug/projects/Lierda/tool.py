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

        pair = KeyValuePair("BARCODE",device.deviceId)
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


