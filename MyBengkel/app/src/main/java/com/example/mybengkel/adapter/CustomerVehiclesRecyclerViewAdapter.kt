package com.example.mybengkel.adapter

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.mybengkel.network.models.CustomerVehicleDto

abstract class CustomerVehiclesRecyclerViewAdapter(
    private val data : ArrayList<CustomerVehicleDto>
) : RecyclerView.Adapter<RecyclerView.ViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        val adapter = LayoutInflater.from(parent.context)
            .inflate(viewType, parent, false)
        return viewHolder(adapter, viewType)
    }

    override fun getItemCount(): Int {
        return data.size
    }

    override fun getItemViewType(position: Int): Int {
        return layoutId(position, data[position])
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        (holder as Binder).bindData(data[position])
    }

    abstract fun viewHolder(view: View, viewType: Int) : RecyclerView.ViewHolder

    abstract fun layoutId(position: Int, obj: CustomerVehicleDto) : Int

    internal interface Binder {
        fun bindData(obj: CustomerVehicleDto)
    }
}