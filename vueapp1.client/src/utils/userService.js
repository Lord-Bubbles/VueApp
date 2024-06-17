import { useAuthStore } from '@/stores/authStore';
import { fetchIntercept } from './fetchIntercept';

export async function getUsers(query) {
  const { response, data } = await fetchIntercept(
    `/api/User?type=${query.type}&managerName=${query.managerName}&` +
      `name=${query.name}&minAge=${query.minAge}&maxAge=${query.maxAge}&` +
      `email=${query.email}&page=${query.page}&limit=${query.limit}`
  );
  if (!response.ok) {
    throw new Error();
  }
  return data;
}

export async function deleteUser(id) {
  const { response } = await fetchIntercept(`/api/User/${id}`, {
    method: 'DELETE'
  });
  if (!response.ok) {
    throw new Error();
  }
  const authStore = useAuthStore();
  if (authStore.user.id === id) {
    await authStore.logout();
  }
}

export async function updateUser(params) {
  const body = Object.assign({}, params);
  delete body.confirmPassword;
  const { response, data } = await fetchIntercept(`/api/User/${params.id}`, {
    method: 'PUT',
    body: JSON.stringify(body)
  });
  if (!response.ok) {
    throw new Error();
  }
  const authStore = useAuthStore();
  if (authStore.user.id === params.id) {
    authStore.user = data;
    localStorage.setItem('user', JSON.stringify(data));
  }
  return data;
}
