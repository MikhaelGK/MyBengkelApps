package com.example.mybengkel.network.models

data class TrxDto(
    var id : String,
    var customerId : String,
    var employeeId : String,
    var vehicleId : String,
    var description : String,
    var cost : Int
)
