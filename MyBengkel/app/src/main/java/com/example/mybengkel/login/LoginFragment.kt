package com.example.mybengkel.login

import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.util.Base64
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.navigation.fragment.findNavController
import com.example.mybengkel.R
import com.example.mybengkel.databinding.FragmentLoginBinding
import com.example.mybengkel.home.HomeActivity
import com.example.mybengkel.network.models.LoginDto
import com.example.mybengkel.network.Api
import org.json.JSONObject
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

private const val TAG = "LOGINFRAGMENT"

class LoginFragment : Fragment() {
    private var _binding: FragmentLoginBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentLoginBinding.inflate(layoutInflater)
        val root = binding.root
        loadToken()
        login()
        goToRegister()
        return root
    }

    private fun login() {
        binding.btnLogin.setOnClickListener {
            Api.retrofitService.postLogin(
                LoginDto(
                "", "", "",
                binding.edEmi.text.toString(),
                binding.edPwd.text.toString())
            ).enqueue(object: Callback<String> {
                override fun onResponse(call: Call<String>, response: Response<String>) {
                    var token = response.body()
                    Log.d(TAG, token.toString())
                    if (response.code() == 200) {
                        (activity as LoginActivity).saveToken(token.toString())
                        Toast.makeText(activity?.applicationContext, "Login Success", Toast.LENGTH_SHORT).show()
                        val intent = Intent(activity?.applicationContext, HomeActivity::class.java)
                        intent.putExtra("email", binding.edEmi.text.toString())
                        startActivityForResult(intent, 1)
                    }
                    if (response.code() == 404) {
                        Toast.makeText(activity?.applicationContext, "User Not Found", Toast.LENGTH_SHORT).show()
                    }
                    if (response.code() == 400) {
                        Toast.makeText(activity?.applicationContext, "Error", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onFailure(call: Call<String>, t: Throwable) {
                    Toast.makeText(activity?.applicationContext, t.message, Toast.LENGTH_SHORT).show()
                }
            })
        }
    }
    private fun goToRegister() {
        binding.btnRegister.setOnClickListener {
            findNavController().navigate(R.id.action_loginFragment_to_registerFragment)
        }
    }

    private fun loadToken(){
        val preference = activity?.getSharedPreferences("LOGIN", Context.MODE_PRIVATE)
        val token = preference?.getString("token", "No Token").toString()
        if (token != "No Token") {
            val email = getPayload(token)
            val intent = Intent(activity?.applicationContext, HomeActivity::class.java)
            intent.putExtra("email", email)
            startActivityForResult(intent, 1)
        }
    }

    private fun getPayload(token: String) : String {
        val byte = Base64.decode(token.split(".")[1], Base64.URL_SAFE)
        val payload = JSONObject(byte.map {Char(it.toInt())}.joinToString (""))
        return payload.getString("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")
    }
}