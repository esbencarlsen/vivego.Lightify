# vivego.Lightify
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)]()(https://github.com/esbencarlsen/vivego.Lightify/blob/master/LICENSE)

C#/.NET client for the Osram Lightify API that targets netstandard2.0, net47 and net461

All it can do is turn lights on/off and set color via the Osram Lightify gateway

Constants and model are from https://github.com/dfensgmbh/biz.dfch.CS.Osram.Lightify.Client

Username is the same as used in the Osram App

Password is the same as used in the Osram App

Serial Number:

On the Osram gateway on the back there is a serialnumber in the format of: OSRXXXXXXXX-YY
In the Osram REST api the format of the serial number must be in the format of: OSRXXXXXXXX  where "-YY" is removed.


Tested on OSX and Windows10