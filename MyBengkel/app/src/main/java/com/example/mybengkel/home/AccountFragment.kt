package com.example.mybengkel.home

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import com.example.mybengkel.AccountEditActivity
import com.example.mybengkel.databinding.FragmentAccountBinding
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.viewmodel.FragmentViewModel

private const val TAG = "ACCOUNTFRAGMENT"

class AccountFragment : Fragment() {

    private var _binding : FragmentAccountBinding? = null
    private val binding get() = _binding!!
    private var user: Customers = FragmentViewModel.user

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentAccountBinding.inflate(layoutInflater)
        binding.tvUName.text = user.name
        binding.tvUId.text = user.customerId

        binding.profileSettings.setOnClickListener {
            val intent = Intent(activity?.applicationContext, AccountEditActivity::class.java)
            startActivityForResult(intent, 109)
        }

        binding.btnLogout.setOnClickListener {
            activity?.setResult(102)
            activity?.finish()
        }

        return binding.root
    }
}