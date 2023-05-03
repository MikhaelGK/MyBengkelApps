package com.example.mybengkel.adapter

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.mybengkel.network.models.DetailTrxDto
import org.json.JSONObject

abstract class TrxesRecyclerViewAdapter(
    private val data: ArrayList<DetailTrxDto>
) : RecyclerView.Adapter<RecyclerView.ViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        val adapter = LayoutInflater.from(parent.context).inflate(viewType, parent, false)
        return viewHolder(adapter, viewType)
    }

    var onItemClick : ((DetailTrxDto) -> Unit)? = null

    override fun getItemViewType(position: Int): Int {
        return layoutId(position, data[position])
    }

    override fun getItemCount(): Int {
        return data.size
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        (holder as Binder).bindData(data[position])
    }

    abstract fun viewHolder(view: View, viewType: Int) : RecyclerView.ViewHolder

    abstract fun layoutId(position: Int, obj: DetailTrxDto) : Int

    internal interface Binder {
        fun bindData(obj: DetailTrxDto)
    }
}