import { useAuthStore } from '@/stores/authStore';

const originalRequest = async (url, config) => {
  const response = await fetch(url, config);
  const data = await response.json();
  return { response, data };
};

export const fetchIntercept = async (url, config = {}) => {
  const authStore = useAuthStore();

  config['headers'] = {
    Authorization: `Bearer ${authStore.token}`,
    'Content-Type': 'application/json',
    Accept: 'application/json'
  };

  let { response, data } = await originalRequest(url, config);

  if (response.statusText === 'Unauthorized') {
    await authStore.refresh();

    config['headers'] = {
      ...config['headers'],
      Authorization: `Bearer ${authStore.token}`
    };

    const newResponse = await originalRequest(url, config);
    response = newResponse.response;
    data = newResponse.data;
  }

  return { response, data };
};
