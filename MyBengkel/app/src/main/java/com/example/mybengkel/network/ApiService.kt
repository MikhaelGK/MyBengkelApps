package com.example.mybengkel.network

import com.example.mybengkel.network.models.LoginDto
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.network.models.Trxes
import com.google.gson.GsonBuilder
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path

private const val BASE_URL = "http://10.0.2.2:5000/api/"

private val gson = GsonBuilder()
    .setLenient()
    .create()

private val retrofit = Retrofit.Builder()
    .addConverterFactory(GsonConverterFactory.create(gson))
    .baseUrl(BASE_URL)
    .build()

interface ApiService {
    @GET("Customers/{email}")
    fun getCustomer(@Path("email") email: String) : Call<Customers>

    @GET("DetailTrxes/{id}")
    fun getDetail(@Path("id") id: String) : Call<Trxes>

    @GET("HeaderTrxes/{id}")
    fun getHeader(@Path("id") id: String) : Call<Trxes>

    @POST("Auth/Login")
    fun postLogin(@Body user: LoginDto) : Call<String>

    @POST("Auth/Register")
     fun postRegister(@Body user: LoginDto) : Call<String>

    @POST("Trxes")
     fun addTransaction(@Body trx: Trxes)

    @PUT("Trxes")
     fun updateTransaction(@Body trx: Trxes)

    @PUT("Customers/{id}")
    fun updateUser(@Body user: Customers) : Call<String>
}

object Api {
    val retrofitService: ApiService by lazy {
        retrofit.create(ApiService::class.java)
    }
}