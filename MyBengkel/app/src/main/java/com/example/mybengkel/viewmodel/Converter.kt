package com.example.mybengkel.viewmodel

import java.text.NumberFormat
import java.util.Locale

class Converter {
    companion object {
        fun rupiah(number: Double) : String {
            val localID = Locale("in", "ID")
            val numberFormat = NumberFormat.getCurrencyInstance(localID)
            return numberFormat.format(number).toString()
        }
    }
}