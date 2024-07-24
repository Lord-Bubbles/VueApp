<script setup>
  import ReviewFormModal from '@/components/ReviewFormModal.vue';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';
  import { useAuthStore } from '@/stores/authStore';
  import { getUsers } from '@/utils/userService';
  import { ref } from 'vue';

  const authStore = useAuthStore();

  const modal = ref(false);
  const user = ref(0);

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

  const headers = ['ID', 'First Name', 'Last Name', 'Age', 'Email', 'Birthday', 'Phone Number'];

  const transformData = (data) => {
    const users = data.users.map(
      (u) =>
        new Object({
          id: u.id,
          firstName: u.firstName,
          lastName: u.lastName,
          age: u.age,
          email: u.email,
          birthday: u.birthday,
          phoneNumber: u.phoneNumber
        })
    );
    return {
      ...data,
      users
    };
  };

  const { data } = useQuery({
    queryKey: ['users', params],
    queryFn: () => getUsers(params),
    placeholderData: keepPreviousData,
    select: (data) => transformData(data)
  });

  const setUser = (id) => {
    modal.value = true;
    user.value = id;
  };
</script>

<template>
  <section class="w-75">
    <h1 class="mb-3">My Team Members</h1>
    <div v-if="data?.count" class="table-responsive">
      <table class="table table-striped table-hover table-bordered table-dark">
        <thead>
          <tr>
            <th v-for="(field, index) in headers" :key="field + '-' + index">
              {{ field }}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, index) in data.users" :key="user.id">
            <td v-for="(value, key) in user" :key="key + '-' + index">
              {{ value }}
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
