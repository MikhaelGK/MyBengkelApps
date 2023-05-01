package com.example.mybengkel.login

import android.content.Context
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.mybengkel.databinding.ActivityLoginBinding

class LoginActivity : AppCompatActivity() {

    private lateinit var binding: ActivityLoginBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)
        }

    fun saveToken(token: String) {
        val preference = getSharedPreferences("LOGIN", Context.MODE_PRIVATE)
        val editor = preference?.edit()
        editor?.putString("token", token)
        editor?.commit()
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        if (resultCode == 102) {
            getSharedPreferences("LOGIN", Context.MODE_PRIVATE).edit().clear().commit()
        }
    }
}