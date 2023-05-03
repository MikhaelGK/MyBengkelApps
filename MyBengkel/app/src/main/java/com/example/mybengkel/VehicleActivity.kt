package com.example.mybengkel

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.widget.TextView
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mybengkel.adapter.CustomerVehiclesRecyclerViewAdapter
import com.example.mybengkel.databinding.ActivityVehicleBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.CustomerVehicleDto
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.viewmodel.FragmentViewModel
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class VehicleActivity : AppCompatActivity() {

    private var _binding : ActivityVehicleBinding? = null
    private val binding get() = _binding!!
    private val customerVehicles = FragmentViewModel.customerVehicle

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        _binding = ActivityVehicleBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setRecyclerView()
    }

    private fun setRecyclerView() {
        val adapter = object: CustomerVehiclesRecyclerViewAdapter(customerVehicles) {
            override fun viewHolder(
                view: View,
                viewType: Int
            ): RecyclerView.ViewHolder {
                return object : RecyclerView.ViewHolder(view), Binder {
                    override fun bindData(obj: CustomerVehicleDto) {
                        val tvCVId =
                            view.findViewById<TextView>(R.id.tvCustomerVehicleId)
                        val tvVId = view.findViewById<TextView>(R.id.tvVehicleId)
                        val tvVName = view.findViewById<TextView>(R.id.tvVehicleName)
                        val tvVNumber =
                            view.findViewById<TextView>(R.id.tvVehicleNumber)

                        tvCVId.text = obj.customerVehicleId.toString()
                        tvVId.text = obj.vehicleId
                        tvVName.text = obj.vehicleName
                        tvVNumber.text = obj.vehicleNumber
                    }

                }
            }

            override fun layoutId(position: Int, obj: CustomerVehicleDto): Int {
                return R.layout.item_recyclerview_customervehicle
            }

        }
        binding.rvVehicle.adapter = adapter
        binding.rvVehicle.setHasFixedSize(true)
    }
}