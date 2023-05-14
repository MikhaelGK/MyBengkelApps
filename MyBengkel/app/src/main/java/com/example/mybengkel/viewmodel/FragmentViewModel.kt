package com.example.mybengkel.viewmodel

import com.example.mybengkel.network.models.CustomerVehicleDto
import com.example.mybengkel.network.models.Customers

class FragmentViewModel {

    companion object {
        var user = Customers()
        var customerVehicle = ArrayList<CustomerVehicleDto>()
    }

}