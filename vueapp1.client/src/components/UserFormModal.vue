<script setup>
  import { useMutation, useQueryClient } from '@tanstack/vue-query';
  import { deleteUser, updateUser } from '@/utils/userService';
  import AutoComplete from './AutoComplete.vue';
  import { useForm, Field, ErrorMessage } from 'vee-validate';
  import { date, object, string } from 'yup';
  import { useToast } from 'vue-toastification';

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

  const { isSubmitting, handleSubmit, setFieldError } = useForm({
    validateOnMount: false,
    validationSchema: object().shape({
      firstName: string().required('First name is required'),
      lastName: string().required('Last name is required'),
      email: string().email('Email must be valid').required('Email is required'),
      birthday: date().max(new Date(), 'Birthday can not be a future date'),
      phoneNum: string().matches(/^\d{3}-\d{3}-\d{4}$/, {
        message: 'Phone number must be in the format xxx-xxx-xxxx',
        excludeEmptyString: true
      }),
      managerName: string().required('Manager name is required')
    }),
    initialValues: {
      firstName: props.data?.firstName,
      lastName: props.data?.lastName,
      email: props.data?.email,
      birthday: props.data?.birthday,
      phoneNum: props.data?.phoneNum,
      accountType: props.data?.accountType,
      managerName: props.data?.managerName
    }
  });

  const queryClient = useQueryClient();

  const { mutate } = useMutation({
    mutationFn: (params) => {
      props.mode === 'delete' ? deleteUser(params) : updateUser(props.data?.id, params);
    },
    onSuccess: async () => {
      modal.value = false;
      return await queryClient.invalidateQueries({
        queryKey: ['users'],
        refetchType: 'all'
      });
    },
    onError: (error) => {
      const toast = useToast();
      if (error.message == 'Bad Request') {
        setFieldError('email', 'A user with this email already exists');
      } else {
        toast.error('An unexpected error has occurred while modifying user');
      }
    }
  });

  const updateData = handleSubmit((values) => {
    if (props.mode === 'delete') {
      mutate(props.data.id);
    } else {
      mutate(values);
    }
  });
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
          <form @submit="updateData">
            <span v-if="props.mode === 'delete'">Are you sure you want to delete this user?</span>
            <div v-else>
              <div class="row mb-3">
                <div class="col">
                  <label class="form-label">First Name</label>
                  <Field class="form-control" name="firstName" />
                  <ErrorMessage class="text-danger" name="firstName" />
                </div>
                <div class="col">
                  <label class="form-label">Last Name</label>
                  <Field class="form-control" name="lastName" />
                  <ErrorMessage class="text-danger" name="lastName" />
                </div>
              </div>
              <div class="mb-3">
                <label class="form-label">Email</label>
                <Field class="form-control" name="email" />
                <ErrorMessage class="text-danger" name="email" />
              </div>
              <div class="row mb-3">
                <label class="form-label">Phone Number</label>
                <Field type="tel" class="form-control" name="phoneNum" />
                <ErrorMessage class="text-danger" name="phoneNum" />
              </div>
              <div class="row mb-3">
                <div class="col">
                  <label class="form-label">Birthday</label>
                  <Field type="date" class="form-control" name="birthday" />
                  <ErrorMessage class="text-danger" name="birthday" />
                </div>
                <div class="col">
                  <label class="form-label">Employee Type</label>
                  <Field as="select" class="form-select" name="accountType">
                    <option>Admin</option>
                    <option>Manager</option>
                    <option>Employee</option>
                  </Field>
                  <ErrorMessage class="text-danger" name="accountType" />
                </div>
              </div>
              <AutoComplete name="managerName" />
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
                {{ props.mode === 'delete' ? 'Delete' : isSubmitting ? 'Saving...' : 'Save' }}
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
