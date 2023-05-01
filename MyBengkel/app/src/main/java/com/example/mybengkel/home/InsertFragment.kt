package com.example.mybengkel.home

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import com.example.mybengkel.R
import com.example.mybengkel.databinding.FragmentAccountBinding
import com.example.mybengkel.databinding.FragmentHistoryBinding
import com.example.mybengkel.databinding.FragmentInsertBinding


class InsertFragment : Fragment() {

    private var _binding : FragmentInsertBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentInsertBinding.inflate(layoutInflater)
        return binding.root
    }
}