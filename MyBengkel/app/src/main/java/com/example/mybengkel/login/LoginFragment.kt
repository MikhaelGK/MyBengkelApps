package com.example.mybengkel.login

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.FragmentActivity
import androidx.navigation.fragment.findNavController
import com.example.mybengkel.R
import com.example.mybengkel.databinding.FragmentLoginBinding
import com.example.mybengkel.models.Users
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.ApiService
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class LoginFragment : Fragment() {

    private var _binding: FragmentLoginBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentLoginBinding.inflate(layoutInflater)
        val root = binding.root

        binding.btnLogin.setOnClickListener {
            val user = Users(
                "",
                "",
                binding.edEmi.text.toString(),
                binding.edPwd.text.toString(),
                "",
                "")
            Api.retrofitService.postLogin(user).enqueue(
                object: Callback<String>  {
                    override fun onResponse(call: Call<String>, response: Response<String>) {
                        val responseCode = response.code();
                        if (responseCode == 200)
                        {
                            val token = response.body()
                            findNavController().navigate(R.id.action_loginFragment_to_homeActivity)
                        }

                    }

                    override fun onFailure(call: Call<String>, t: Throwable) {
                        Toast.makeText(context, t.toString(), Toast.LENGTH_SHORT).show()
                    }

                })


        }

        binding.btnRegister.setOnClickListener {
            findNavController().navigate(R.id.action_loginFragment_to_registerFragment)
        }

        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

    }
}