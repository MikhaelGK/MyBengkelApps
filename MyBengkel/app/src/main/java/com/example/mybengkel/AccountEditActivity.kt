package com.example.mybengkel

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.mybengkel.R
import com.example.mybengkel.databinding.ActivityAccountEditBinding
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.viewmodel.FragmentViewModel

class AccountEditActivity : AppCompatActivity() {

    private var _binding : ActivityAccountEditBinding? = null
    private val binding get() = _binding!!
    private var user: Customers = FragmentViewModel.user

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        _binding = ActivityAccountEditBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.tvCustomerId.text = user.customerId
        binding.edName.setText(user.name)
        binding.edEmi.setText(user.email)
        binding.edPwd.setText(user.password)
        binding.edAds.setText(user.address)
        binding.edPho.setText(user.phone)

        binding.btnSave.setOnClickListener {



            setResult(302)
            finish()
        }
    }
}