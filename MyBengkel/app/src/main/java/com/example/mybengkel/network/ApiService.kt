package com.example.mybengkel.network

import com.example.mybengkel.network.models.CustomerVehicleDto
import com.example.mybengkel.network.models.LoginDto
import com.example.mybengkel.network.models.Customers
import com.example.mybengkel.network.models.DetailTrxDto
import com.example.mybengkel.network.models.TrxDto
import com.google.gson.GsonBuilder
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.Body
import retrofit2.http.Field
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import retrofit2.http.Query

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
    fun getDetail(@Path("id") id: String) : Call<DetailTrxDto>

    @GET("HeaderTrxes/{id}")
    fun getHeader(
        @Path("id") id: String,
        @Query("query") query: String?
    ) : Call<ArrayList<DetailTrxDto>>

    @GET("CustomerVehicles/{customerId}")
    fun getCustomerVehicles(@Path("customerId") customerId : String) : Call<ArrayList<CustomerVehicleDto>>

    @POST("Auth/Login")
    fun postLogin(@Body user: LoginDto) : Call<String>

    @POST("Auth/Register")
     fun postRegister(@Body user: LoginDto) : Call<String>

    @POST("Trxes")
     fun postTransaction(@Body trx: TrxDto) : Call<String>

    @PUT("Trxes")
     fun updateTransaction(@Body trx: DetailTrxDto)

    @PUT("Customers/{id}")
    fun updateUser(
        @Path("id") id: String,
        @Body user: Customers) : Call<String>
}

object Api {
    val retrofitService: ApiService by lazy {
        retrofit.create(ApiService::class.java)
    }
}