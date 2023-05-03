package com.example.mybengkel.login

import android.content.Intent
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import com.example.mybengkel.databinding.FragmentRegisterBinding
import com.example.mybengkel.home.HomeActivity
import com.example.mybengkel.network.models.LoginDto
import com.example.mybengkel.network.Api
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

private const val TAG = "REGISTERFRAGMENT"

class RegisterFragment : Fragment() {

    private var _binding : FragmentRegisterBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentRegisterBinding.inflate(layoutInflater)
        register()
        return binding.root
    }

    private fun register() {
        binding.btnRegister.setOnClickListener {
            var user = LoginDto(
                binding.edName.text.toString(),
                binding.edAds.text.toString(),
                binding.edPho.text.toString(),
                binding.edEmi.text.toString(),
                binding.edPwd.text.toString()
            )
            Api.retrofitService.postRegister(user).enqueue(object: Callback<String> {
                override fun onResponse(call: Call<String>, response: Response<String>) {
                    if (response.code() == 200) {
                        var token = response.body()
                        Log.d(TAG, token.toString())
                        Toast.makeText(activity?.applicationContext, "Register Success", Toast.LENGTH_SHORT).show()
                        (activity as LoginActivity).saveToken(token.toString())
                        val intent = Intent(activity?.applicationContext, HomeActivity::class.java)
                        intent.putExtra("email", binding.edEmi.text.toString())
                        startActivityForResult(intent, 1)
                    }
                    if (response.code() == 400){
                        Toast.makeText(activity?.applicationContext, "Error", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onFailure(call: Call<String>, t: Throwable) {
                    Toast.makeText(activity?.applicationContext, t.message, Toast.LENGTH_SHORT).show()
                }

            })
        }
    }
}