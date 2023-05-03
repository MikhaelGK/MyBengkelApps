package com.example.mybengkel

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import com.example.mybengkel.databinding.ActivityDetailTrxBinding
import com.example.mybengkel.network.Api
import com.example.mybengkel.network.models.DetailTrxDto
import com.example.mybengkel.viewmodel.Converter
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class DetailTrxActivity : AppCompatActivity() {

    private var _binding : ActivityDetailTrxBinding? = null
    private val binding get() = _binding!!


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        _binding = ActivityDetailTrxBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val trxId = intent.getStringExtra("trxId").toString()
        Api.retrofitService.getDetail(trxId)
            .enqueue(object: Callback<DetailTrxDto> {
                override fun onResponse(
                    call: Call<DetailTrxDto>,
                    response: Response<DetailTrxDto>
                ) {
                    if (response.code() == 200) {
                        val data = response.body()!!
                        binding.tvTrxId.text = data.trxId
                        binding.tvDate.text = data.date
                        binding.tvVehicleName.text = data.vehicleName
                        binding.tvVehicleNumber.text = data.vehicleNumber
                        binding.tvCost.text = Converter.rupiah(data.cost.toDouble())
                    }
                }

                override fun onFailure(call: Call<DetailTrxDto>, t: Throwable) {
                    Toast.makeText(applicationContext, t.message, Toast.LENGTH_SHORT).show()
                }

            })
    }
}