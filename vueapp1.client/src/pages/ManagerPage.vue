<script setup>
  import ReviewFormModal from '@/components/ReviewFormModal.vue';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';
  import { useAuthStore } from '@/stores/authStore';
  import { getUsers } from '@/utils/userService';
  import { ref } from 'vue';

  const authStore = useAuthStore();

  const modal = ref(false);
  const user = ref(0);

  const headers = ['First Name', 'Last Name', 'Age', 'Email', 'Birthday', 'Phone Number'];
  const fields = ['firstName', 'lastName', 'age', 'email', 'birthday', 'phoneNumber'];

  const params = {
    name: '',
    minAge: 0,
    maxAge: 0,
    managerName: authStore.user.firstName + ' ' + authStore.user.lastName,
    type: '',
    email: '',
    page: 1,
    limit: 1000
  };

  const { data } = useQuery({
    queryKey: ['users'],
    queryFn: () => getUsers(params),
    placeholderData: keepPreviousData
  });

  const setUser = (id) => {
    modal.value = true;
    user.value = id;
  };
</script>

<template>
  <section class="container m-4 p-4">
    <h1 class="mb-3">My Team Members</h1>
    <div v-if="data && data.count > 0" class="table-responsive">
      <table class="table table-striped table-hover table-bordered table-dark">
        <thead>
          <tr>
            <th v-for="field in headers" :key="'team-' + field">
              {{ field }}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, index) of data.users" :key="user.id">
            <td v-for="field in fields" :key="field + '-' + index">
              {{ user[field] }}
            </td>
            <td>
              <button class="btn btn-primary" type="button" @click="setUser(user.id)">
                Add Review
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <ReviewFormModal v-if="modal" v-model="modal" type="manager" :userID="user" />
    </div>
  </section>
</template>
