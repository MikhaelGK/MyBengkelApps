package com.example.mybengkel.home

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.appcompat.widget.SearchView
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.RecyclerView
import com.example.mybengkel.DetailTrxActivity
import com.example.mybengkel.R
import com.example.mybengkel.adapter.TrxesRecyclerViewAdapter
import com.example.mybengkel.databinding.FragmentHistoryBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.DetailTrxDto
import com.example.mybengkel.viewmodel.Converter
import com.example.mybengkel.viewmodel.FragmentViewModel
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response


class HistoryFragment : Fragment() {

    private var _binding : FragmentHistoryBinding? = null
    private val binding get() = _binding!!
    private val user = FragmentViewModel.user
    private val header = ArrayList<DetailTrxDto>()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentHistoryBinding.inflate(layoutInflater)

        binding.searchView.setOnQueryTextListener(
            object: SearchView.OnQueryTextListener {
                override fun onQueryTextSubmit(query: String?): Boolean {
                    val search = query.toString()
                    getHeaderTrx(search)
                    return false
                }

                override fun onQueryTextChange(query: String?): Boolean {
                    val search = query.toString()
                    getHeaderTrx(search)
                    return true
                }

            }
        )

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        getHeaderTrx("")
    }

    private fun getHeaderTrx(query: String) {
        Api.retrofitService.getHeader(user.customerId, query)
            .enqueue(object: Callback<ArrayList<DetailTrxDto>> {
                override fun onResponse(
                    call: Call<ArrayList<DetailTrxDto>>,
                    response: Response<ArrayList<DetailTrxDto>>
                ) {
                    if (response.code() == 200) {
                        header.clear()
                        response.body()?.let { header.addAll(it) }
                        val adapter = object: TrxesRecyclerViewAdapter(header) {
                            override fun viewHolder(
                                view: View,
                                viewType: Int
                            ): RecyclerView.ViewHolder {
                                return object: RecyclerView.ViewHolder(view), Binder {
                                    override fun bindData(obj: DetailTrxDto) {
                                        val tvTrxId = view.findViewById<TextView>(R.id.tvTrxId)
                                        val tvDate = view.findViewById<TextView>(R.id.tvDate)
                                        val tvCost = view.findViewById<TextView>(R.id.tvCost)



                                        tvTrxId.text = obj.trxId
                                        tvDate.text = obj.date
                                        tvCost.text = Converter.rupiah(obj.cost.toDouble())

                                        view.setOnClickListener {
                                            onItemClick?.invoke(obj)
                                        }
                                    }
                                }
                            }

                            override fun layoutId(position: Int, obj: DetailTrxDto): Int {
                                return R.layout.item_recyclerview_history
                            }
                        }
                        binding.rvHistory.adapter = adapter
                        binding.rvHistory.setHasFixedSize(true)

                        adapter.onItemClick = {
                            val intent = Intent(activity?.applicationContext, DetailTrxActivity::class.java)
                            intent.putExtra("trxId", it.trxId)
                            startActivityForResult(intent, 903)
                        }
                    }
                }

                override fun onFailure(call: Call<ArrayList<DetailTrxDto>>, t: Throwable) {
                    TODO("Not yet implemented")
                }
            })
    }

}