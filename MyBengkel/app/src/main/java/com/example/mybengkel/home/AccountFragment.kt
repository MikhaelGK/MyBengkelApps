package com.example.mybengkel.home

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import com.example.mybengkel.AccountEditActivity
import com.example.mybengkel.VehicleActivity
import com.example.mybengkel.databinding.FragmentAccountBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.CustomerVehicleDto
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.viewmodel.FragmentViewModel
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

private const val TAG = "ACCOUNTFRAGMENT"

class AccountFragment : Fragment() {

    private var _binding : FragmentAccountBinding? = null
    private val binding get() = _binding!!
    private var user: Customers = FragmentViewModel.user
    private val vehicles = FragmentViewModel.customerVehicle

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentAccountBinding.inflate(layoutInflater)
        binding.tvUName.text = user.name
        binding.tvUId.text = user.customerId

        binding.totalVehicle.text = vehicles.size.toString()
        binding.profileSettings.setOnClickListener {
            val intent = Intent(activity?.applicationContext, AccountEditActivity::class.java)
            startActivityForResult(intent, 109)
        }

        binding.btnLogout.setOnClickListener {
            activity?.setResult(102)
            activity?.finish()
        }

        binding.btnMyVehicle.setOnClickListener {
            val intent = Intent(activity?.applicationContext, VehicleActivity::class.java)
            startActivity(intent)
        }

        return binding.root
    }
}