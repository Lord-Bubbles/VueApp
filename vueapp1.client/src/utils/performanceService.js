import { useToast } from 'vue-toastification';
import { fetchIntercept } from './fetchIntercept';

const toast = useToast();

export async function getPerformances(query) {
  const { response, data } = await fetchIntercept(
    `/api/Performance?user=${query.user}&type=${query.type}&page=${query.page}&limit=${query.limit}`
  );
  if (!response.ok) {
    throw new Error();
  }
  return data;
}

export async function createPerformance(formData) {
  const { response } = await fetchIntercept('/api/Performance', {
    method: 'POST',
    body: JSON.stringify(formData)
  });
  if (!response.ok) {
    throw new Error();
  }
  toast.success('Performance created successfully');
}
