import axios from 'axios'

// Axios Config
const httpAgent = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_URL,
})

export const setAuthTokenInterceptor = (token: string) => {
  console.log('token', token)
  httpAgent.interceptors.request.use(
    (config) => {
      config.headers['Authorization'] = token;
      return config
    },
    (error) => {
      return Promise.reject(error)
    },
  );
}

httpAgent.interceptors.response.use(undefined, (error) => {
  if (error.message === 'Network Error' && !error.response) {
    console.log('Network error -make sure api is running!')
    return
  }

  if (error.response?.status) {
    const { status } = error.response

    if (status === 500) console.log('Server error')

    throw error.response
  }

  throw error
})

// ADAPTER
export const requestMap = {
  
  async get<TResponse>(url: string, params?: any) : Promise<TResponse> {
    const response: TResponse = await httpAgent.get(url, { params: { ...params }, });
    return response;
  },

  async post<TResponse>(url: string, body: {}): Promise<TResponse> {
    const response: TResponse = await httpAgent.post(url, body);
    return response;
  },

  async put<TResponse> (url: string, body: {}) { 
    const response:TResponse = await httpAgent.put(url, body); 
    return response
  },

  async delete<TResponse> (url: string, params?: any): Promise<TResponse> {
    const response: TResponse = await httpAgent.delete(url, { params: { ...params } });
    return response;
  },

  async postFile<TResponse> (url: string, media: any): Promise<TResponse> {
    const formData = new FormData();
    formData.append('image', media);
    const response: TResponse = await httpAgent
      .post(url, formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });

    return response
  }
}
