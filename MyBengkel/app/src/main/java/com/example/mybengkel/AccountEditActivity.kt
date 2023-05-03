package com.example.mybengkel

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import com.example.mybengkel.R
import com.example.mybengkel.databinding.ActivityAccountEditBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.viewmodel.FragmentViewModel
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

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

            user.name = binding.edName.text.toString()
            user.email = binding.edEmi.text.toString()
            user.password = binding.edPwd.text.toString()
            user.address = binding.edAds.text.toString()
            user.phone = binding.edPho.text.toString()
            Api.retrofitService.updateUser(user.customerId, user).enqueue(object: Callback<String> {
                override fun onResponse(call: Call<String>, response: Response<String>) {
                    Toast.makeText(applicationContext, response.body().toString(), Toast.LENGTH_SHORT).show()
                    if (response.code() == 200) {
                        setResult(302)
                        finish()
                    }
                }

                override fun onFailure(call: Call<String>, t: Throwable) {
                    Toast.makeText(applicationContext, t.toString(), Toast.LENGTH_SHORT).show()
                }

            })


        }
    }
}