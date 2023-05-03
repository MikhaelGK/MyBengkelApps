package com.example.mybengkel.home

import android.app.Dialog
import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.graphics.drawable.ColorDrawable
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.AttributeSet
import android.util.Log
import android.view.Gravity
import android.view.View
import android.view.ViewGroup
import android.view.Window
import android.widget.LinearLayout
import android.widget.Toast
import androidx.core.view.forEach
import androidx.fragment.app.Fragment
import com.example.mybengkel.R
import com.example.mybengkel.databinding.ActivityHomeBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.CustomerVehicleDto
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.viewmodel.FragmentViewModel
import com.google.android.material.bottomnavigation.BottomNavigationView
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

private const val TAG = "HOMEACTIVITY"

class HomeActivity : AppCompatActivity() {

    private var _binding: ActivityHomeBinding? = null
    private val binding get() = _binding!!

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        _binding = ActivityHomeBinding.inflate(layoutInflater)
        setContentView(binding.root)
        getCustomer()
        replaceFragment(HomeFragment())
        bottomNavigationSetOnItemSelectedListener()
        bottomNavigationSetOnLongClickListener()
    }

    private fun getCustomerVehicle(customerId: String) {
        val customerVehicle = ArrayList<CustomerVehicleDto>()
        Api.retrofitService.getCustomerVehicles(customerId)
            .enqueue(object: Callback<ArrayList<CustomerVehicleDto>> {
                override fun onResponse(
                    call: Call<ArrayList<CustomerVehicleDto>>,
                    response: Response<ArrayList<CustomerVehicleDto>>
                ) {
                    if (response.code() == 200) {
                        response.body()?.let {customerVehicle.addAll(it)}
                        FragmentViewModel.customerVehicle = customerVehicle
                    }
                }

                override fun onFailure(call: Call<ArrayList<CustomerVehicleDto>>, t: Throwable) {
                    TODO("Not yet implemented")
                }
            })
    }
    private fun getCustomer() {
        val email = intent.getStringExtra("email").toString()
        Api.retrofitService.getCustomer(email).enqueue(object : Callback<Customers> {
            override fun onResponse(call: Call<Customers>, response: Response<Customers>) {
                if (response.code() == 200) {
                    FragmentViewModel.user.customerId = response.body()?.customerId.toString()
                    FragmentViewModel.user.name = response.body()?.name.toString()
                    FragmentViewModel.user.email = response.body()?.email.toString()
                    FragmentViewModel.user.password = response.body()?.password.toString()
                    FragmentViewModel.user.address = response.body()?.address.toString()
                    FragmentViewModel.user.phone = response.body()?.phone.toString()
                    getCustomerVehicle(FragmentViewModel.user.customerId)
                    Log.d(TAG, FragmentViewModel.user.toString())
                }
                if (response.code() == 400) {
                    Toast.makeText(applicationContext, "Error", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<Customers>, t: Throwable) {
                Toast.makeText(applicationContext, "Error", Toast.LENGTH_SHORT).show()
            }

        })
    }
    private fun showDialog(context: Context) {
        val dialog = Dialog(context)
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE)
        dialog.setContentView(R.layout.bottom_sheet_layout)

        val logoutLayout = dialog.findViewById<LinearLayout>(R.id.layoutLogout)
        logoutLayout.setOnClickListener {
            setResult(102)
            finish()
        }

        dialog.show()
        dialog.window?.setLayout(
            ViewGroup.LayoutParams.MATCH_PARENT,
            ViewGroup.LayoutParams.WRAP_CONTENT
        )
        dialog.window?.setBackgroundDrawable(ColorDrawable(Color.TRANSPARENT))
        dialog.window?.attributes?.windowAnimations = R.style.DialogAnimation
        dialog.window?.setGravity(Gravity.BOTTOM)
    }
    private fun bottomNavigationSetOnLongClickListener() {

        binding.bottomNavigation.findViewById<View>(R.id.navigation_account)
            .setOnLongClickListener {
                showDialog(this)
                true
            }
    }
    private fun bottomNavigationSetOnItemSelectedListener() {
        binding.bottomNavigation.setOnItemSelectedListener {
            when (it.itemId) {
                R.id.navigation_home -> replaceFragment(HomeFragment())
                R.id.navigation_history -> replaceFragment(HistoryFragment())
                R.id.navigation_account -> replaceFragment(AccountFragment())
                R.id.navigation_insert -> replaceFragment(InsertFragment())
                else -> {}
            }

            true
        }

    }
    private fun replaceFragment(fragment: Fragment) {
        val fragmentManager = supportFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()
        fragmentTransaction.replace(R.id.fragment, fragment)
        fragmentTransaction.commit()
    }
    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        if (resultCode == 302) {
            replaceFragment(AccountFragment())
            getCustomer()
        }
    }
}