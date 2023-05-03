package com.example.mybengkel.home

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.mybengkel.databinding.FragmentInsertBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.TrxDto
import com.example.mybengkel.viewmodel.FragmentViewModel
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

private const val TAG = "INSERTFRAGMENT"

class InsertFragment : Fragment() {

    private var _binding : FragmentInsertBinding? = null
    private val binding get() = _binding!!
    private val vehicle = FragmentViewModel.customerVehicle
    private val user = FragmentViewModel.user
    private var vehicleName: String = ""
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentInsertBinding.inflate(layoutInflater)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val vehicleNameList = ArrayList<String>()
        vehicle.map { vehicleNameList.add(it.vehicleName) }
        val adapter = ArrayAdapter<String>(
            this.requireContext(),
            androidx.constraintlayout.widget.R.layout.support_simple_spinner_dropdown_item,
            vehicleNameList
            )
        binding.spinner.adapter = adapter
        binding.spinner.onItemSelectedListener = object: AdapterView.OnItemSelectedListener {
            override fun onItemSelected(adapterView: AdapterView<*>?, view: View?, position: Int, id: Long) {
                vehicleName = adapterView?.getItemAtPosition(position).toString()
                Toast.makeText(activity?.applicationContext,
                    vehicleName,
                    Toast.LENGTH_SHORT).show()
            }

            override fun onNothingSelected(p0: AdapterView<*>?) {
                TODO("Not yet implemented")
            }

        }

        binding.btnInsert.setOnClickListener {

            val selectedVehicle = vehicle.filter { it.vehicleName == vehicleName }[0]

            var trx = TrxDto(
                "",
                user.customerId,
                "E00001",
                selectedVehicle.vehicleId,
                binding.edDesc.text.toString(),
                0
            )
            Api.retrofitService.postTransaction(trx)
                .enqueue(object: Callback<String> {
                    override fun onResponse(call: Call<String>, response: Response<String>) {
                        if (response.code() == 200) {
                            Toast.makeText(activity?.applicationContext, response.body() + ", your vehicle will be repaired soon", Toast.LENGTH_SHORT)
                                .show()
                            binding.edDesc.setText("")
                        }
                    }

                    override fun onFailure(call: Call<String>, t: Throwable) {
                        TODO("Not yet implemented")
                    }

                })
        }
    }
}