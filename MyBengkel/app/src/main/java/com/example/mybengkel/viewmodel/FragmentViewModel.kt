package com.example.mybengkel.viewmodel

import androidx.lifecycle.ViewModel
import com.example.mybengkel.network.models.Customers

class FragmentViewModel : ViewModel() {

    companion object {
        var user = Customers()
    }

}