# Charger

This repository contains the code for a charger.
The charger is a rack for 3 12V batteries inside and 1 connector outside (for a bigger battery e.g. car-battery).
Normally I built it for 3 motocycle batteries.

The charger has 3 modes: 
Charging - Charge the batteries with 14.1V
Observation - Measure voltage of the connected batteries 
  	and bring them to homeassistant and also integrated Display
Conservation - Measure voltage of the connected batteries and bring them to homeassistant 
    and also integrated Display, compare each voltage with a setpoint and charge with 14.1V if it's necessary

Use case:
Hold all batteries on healthy voltage level (during winter for motocycle use case).

Technologies:
.NET 6 project
MQTT - to communicate with homeassistant
Homeassistant Auto Discovery - to automatically create all entities
Modbus - to cumminicate WagoController
RaspberryPi - to control the whole device
PaspberryPi - to control Display, LEDs and a doorsensor
