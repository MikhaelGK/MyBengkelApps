package com.example.mybengkel.network

import com.example.mybengkel.models.Users
import com.example.mybengkel.models.Trxes
import com.google.gson.GsonBuilder
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT

private const val BASE_URL = "http://10.0.2.2/api/"

private val gson = GsonBuilder()
    .setDateFormat("yyyy-MM-dd")
    .create()

private val retrofit = Retrofit.Builder()
    .addConverterFactory(GsonConverterFactory.create(gson))
    .baseUrl(BASE_URL)
    .build()

interface ApiService {
    @GET("Customers/{email}")
    suspend fun getCustomer() : Call<Users>

    @GET("DetailTrxes/{id}")
    suspend fun getDetail() : Call<Trxes>

    @GET("HeaderTrxes/{id}")
    suspend fun getHeader() : Call<Trxes>

    @POST("Auth/Login")
    fun postLogin(@Body user: Users) : Call<String>

    @POST("Auth/Register")
    suspend fun postRegister(@Body user: Users)

    @POST("Trxes")
    suspend fun addTransaction(@Body trx: Trxes)

    @PUT("Trxes")
    suspend fun updateTransaction(@Body trx: Trxes)

    @PUT("Customers/{id}")
    suspend fun updateUser(@Body user: Users)
}

object Api {
    val retrofitService: ApiService by lazy {
        retrofit.create(ApiService::class.java)
    }
}