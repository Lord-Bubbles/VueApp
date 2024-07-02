import { useAuthStore } from '@/stores/authStore';
import { fetchIntercept } from '@/utils/fetchIntercept';
import { useToast } from 'vue-toastification';

const authStore = useAuthStore();
const toast = useToast();

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
  if (authStore.user.id === id) {
    await authStore.logout();
  }
  toast.success('Deleted user successfully');
}

export async function updateUser(id, params) {
  const body = Object.assign({}, params);
  delete body.confirmPassword;
  const { response, data } = await fetchIntercept(`/api/User/${id}`, {
    method: 'PUT',
    body: JSON.stringify(body)
  });
  if (!response.ok) {
    toast.error('An error occurred while updating user');
    throw new Error();
  }
  if (authStore.user.id === id) {
    authStore.user = data;
  }
  toast.success('Updated user successfully');
}
