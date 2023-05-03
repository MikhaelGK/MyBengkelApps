package com.example.mybengkel.network.models

data class DetailTrxDto(
    val trxId: String,
    val date: String,
    val vehicleName: String,
    val vehicleNumber: String,
    val description: String,
    val cost: Int
)
