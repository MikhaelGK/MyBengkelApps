package com.example.mybengkel

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.mybengkel.login.LoginActivity
import kotlin.concurrent.thread

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        thread {
            Thread.sleep(3000)
            val intent = Intent(this, LoginActivity::class.java)
            startActivity(intent)
        }
    }
}