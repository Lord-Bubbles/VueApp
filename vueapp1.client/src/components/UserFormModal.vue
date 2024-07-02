<script setup>
  import { useMutation, useQueryClient } from '@tanstack/vue-query';
  import { ref } from 'vue';
  import { deleteUser, updateUser } from '@/utils/userService';
  import AutoComplete from './AutoComplete.vue';

  const props = defineProps({
    mode: {
      type: String,
      required: true
    },
    data: {
      type: Object,
      required: true
    }
  });
  const modal = defineModel({ required: true, type: Boolean });

  const formData = ref({
    firstName: props.data?.firstName,
    lastName: props.data?.lastName,
    email: props.data?.email,
    birthday: props.data?.birthday,
    phoneNum: props.data?.phoneNum,
    accountType: props.data?.accountType,
    managerName: props.data?.managerName
  });

  const queryClient = useQueryClient();

  const { mutate } = useMutation({
    mutationFn: (params) => {
      props.mode === 'delete' ? deleteUser(params) : updateUser(props.data?.id, params);
    },
    onSuccess: async () => {
      modal.value = false;
      return await queryClient.invalidateQueries({ queryKey: ['users'], refetchType: 'all' });
    }
  });

  const updateData = () => {
    if (props.mode === 'delete') {
      mutate(formData.value.id);
    } else {
      mutate(formData.value);
    }
  };
</script>

<template>
  <div
    class="modal fade"
    data-bs-backdrop="static"
    tabindex="-1"
    data-bs-keyboard="false"
    :class="{ show: modal, 'd-block': modal }"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content text-light-emphasis">
        <div class="modal-header">
          <button
            type="button"
            class="btn-close"
            @click="() => (modal = false)"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="updateData">
            <span v-if="props.mode === 'delete'">Are you sure you want to delete this user?</span>
            <div v-else>
              <div class="row mb-3">
                <div class="col">
                  <label class="form-label">First Name</label>
                  <input class="form-control" v-model="formData.firstName" />
                </div>
                <div class="col">
                  <label class="form-label">Last Name</label>
                  <input class="form-control" v-model="formData.lastName" />
                </div>
              </div>
              <div class="mb-3">
                <label class="form-label">Email</label>
                <input class="form-control" v-model="formData.email" />
              </div>
              <div class="row mb-3">
                <div class="col">
                  <label class="form-label">Age</label>
                  <input type="number" class="form-control" v-model="formData.age" />
                </div>
                <div class="col">
                  <label class="form-label">Phone Number</label>
                  <input type="tel" class="form-control" v-model="formData.phoneNum" />
                </div>
              </div>
              <div class="row mb-3">
                <div class="col">
                  <label class="form-label">Birthday</label>
                  <input type="date" class="form-control" v-model="formattedBirthday" />
                </div>
                <div class="col">
                  <label class="form-label">Employee Type</label>
                  <select class="form-select" v-model="formData.accountType">
                    <option>Admin</option>
                    <option>Manager</option>
                    <option>Employee</option>
                  </select>
                </div>
              </div>
              <AutoComplete v-model="formData.managerName" />
            </div>
            <div class="d-flex justify-content-evenly my-3">
              <button
                type="submit"
                class="btn"
                :class="{
                  'btn-danger': props.mode === 'delete',
                  'btn-primary': props.mode === 'edit'
                }"
              >
                {{ props.mode === 'delete' ? 'Yes' : 'Save Changes' }}
              </button>
              <button type="button" class="btn btn-secondary" @click="() => (modal = false)">
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>